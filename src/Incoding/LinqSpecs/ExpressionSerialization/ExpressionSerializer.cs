namespace Incoding.ExpressionSerialization
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;

    #endregion

    // This code is taken from the codeplex project Jammer.NET
    // you can find the project under http://jmr.codeplex.com/
    // Here is the URL to the original code: https://jmr.svn.codeplex.com/svn/trunk/Jmr.Silverlight/Serialization/ExpressionSerializer.cs
    public class PropValue
    {
        #region Properties

        public PropertyInfo Property { get; set; }

        public object Value { get; set; }

        #endregion
    }

    public class ExpressionSerializer
    {
        ////ncrunch: no coverage start
        #region Static Fields

        static readonly Type[] attributeTypes = new[] { typeof(string), typeof(int), typeof(bool), typeof(ExpressionType) };

        #endregion

        #region Fields

        readonly Dictionary<string, ParameterExpression> parameters = new Dictionary<string, ParameterExpression>();

        readonly ExpressionSerializationTypeResolver resolver;

        #endregion

        #region Constructors

        public ExpressionSerializer()
        {
            this.resolver = new ExpressionSerializationTypeResolver();
            Converters = new List<CustomExpressionXmlConverter>();
        }

        #endregion

        #region Properties

        public List<CustomExpressionXmlConverter> Converters { get; private set; }

        #endregion

        /*
         * SERIALIZATION 
         */
        #region Api Methods

        public XElement Serialize(Expression e)
        {
            try
            {
                return GenerateXmlFromExpressionCore(e);
            }
            catch (Exception ex) { }
            return null;
        }

        public Expression Deserialize(XElement xml)
        {
            this.parameters.Clear();
            return ParseExpressionFromXmlNonNull(xml);
        }

        public Expression<TDelegate> Deserialize<TDelegate>(XElement xml)
        {
            var e = Deserialize(xml);
            if (e is Expression<TDelegate>)
                return e as Expression<TDelegate>;
            throw new Exception("xml must represent an Expression<TDelegate>");
        }

        #endregion

        XElement GenerateXmlFromExpressionCore(Expression e)
        {
            if (e == null)
                return null;
            var replace = ApplyCustomConverters(e);
            if (replace != null)
                return replace;
            var properties = e.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propList = new List<PropValue>();
            foreach (var prop in properties)
            {
                try
                {
                    var val = prop.GetValue(e, null);
                    propList.Add(new PropValue { Property = prop, Value = val });

                    // return new XElement(GetNameOfExpression(e), GenerateXmlFromProperty(prop.PropertyType, prop.Name, prop.GetValue(e, null)));
                }
                catch (MethodAccessException ex)
                {
                    propList.Add(new PropValue { Property = prop, Value = e.Type });

                    // return new XElement(GetNameOfExpression(e), GenerateXmlFromProperty(prop.PropertyType, prop.Name, null));
                }
            }

            return new XElement(
                    GetNameOfExpression(e), 
                    from prop in propList
                    select GenerateXmlFromProperty(prop.Property.PropertyType, prop.Property.Name, prop.Value));
        }

        XElement ApplyCustomConverters(Expression e)
        {
            foreach (var converter in Converters)
            {
                var result = converter.Serialize(e);
                if (result != null)
                    return result;
            }

            return null;
        }

        string GetNameOfExpression(Expression e)
        {
            if (e is LambdaExpression)
                return "LambdaExpression";
            return XmlConvert.EncodeName(e.GetType().Name);
        }

        object GenerateXmlFromProperty(Type propType, string propName, object value)
        {
            if (attributeTypes.Contains(propType))
                return GenerateXmlFromPrimitive(propName, value);
            if (propType.Equals(typeof(object)))
                return GenerateXmlFromObject(propName, value);
            if (typeof(Expression).IsAssignableFrom(propType))
                return GenerateXmlFromExpression(propName, value as Expression);
            if (value is MethodInfo || propType.Equals(typeof(MethodInfo)))
                return GenerateXmlFromMethodInfo(propName, value as MethodInfo);
            if (value is PropertyInfo || propType.Equals(typeof(PropertyInfo)))
                return GenerateXmlFromPropertyInfo(propName, value as PropertyInfo);
            if (value is FieldInfo || propType.Equals(typeof(FieldInfo)))
                return GenerateXmlFromFieldInfo(propName, value as FieldInfo);
            if (value is ConstructorInfo || propType.Equals(typeof(ConstructorInfo)))
                return GenerateXmlFromConstructorInfo(propName, value as ConstructorInfo);
            if (propType.Equals(typeof(Type)))
                return GenerateXmlFromType(propName, value as Type);
            if (IsIEnumerableOf<Expression>(propType))
                return GenerateXmlFromExpressionList(propName, AsIEnumerableOf<Expression>(value));
            if (IsIEnumerableOf<MemberInfo>(propType))
                return GenerateXmlFromMemberInfoList(propName, AsIEnumerableOf<MemberInfo>(value));
            if (IsIEnumerableOf<ElementInit>(propType))
                return GenerateXmlFromElementInitList(propName, AsIEnumerableOf<ElementInit>(value));
            if (IsIEnumerableOf<MemberBinding>(propType))
                return GenerateXmlFromBindingList(propName, AsIEnumerableOf<MemberBinding>(value));
            throw new NotSupportedException(propName);
        }

        object GenerateXmlFromObject(string propName, object value)
        {
            object result = null;
            if (value is Type)
                result = GenerateXmlFromTypeCore((Type)value);
            if (result == null)
                result = value.ToString();
            return new XElement(propName, 
                                result);
        }

        bool IsIEnumerableOf<T>(Type propType)
        {
            if (!propType.IsGenericType)
                return false;
            var typeArgs = propType.GetGenericArguments();
            if (typeArgs.Length != 1)
                return false;
            if (!typeof(T).IsAssignableFrom(typeArgs[0]))
                return false;
            if (!typeof(IEnumerable<>).MakeGenericType(typeArgs).IsAssignableFrom(propType))
                return false;
            return true;
        }

        IEnumerable<T> AsIEnumerableOf<T>(object value)
        {
            if (value == null)
                return null;
            return (value as IEnumerable).Cast<T>();
        }

        object GenerateXmlFromElementInitList(string propName, IEnumerable<ElementInit> initializers)
        {
            if (initializers == null)
                initializers = new ElementInit[] { };
            return new XElement(propName, 
                                from elementInit in initializers
                                select GenerateXmlFromElementInitializer(elementInit));
        }

        object GenerateXmlFromElementInitializer(ElementInit elementInit)
        {
            return new XElement("ElementInit", 
                                GenerateXmlFromMethodInfo("AddMethod", elementInit.AddMethod), 
                                GenerateXmlFromExpressionList("Arguments", elementInit.Arguments));
        }

        object GenerateXmlFromExpressionList(string propName, IEnumerable<Expression> expressions)
        {
            return new XElement(propName, 
                                from expression in expressions
                                select GenerateXmlFromExpressionCore(expression));
        }

        object GenerateXmlFromMemberInfoList(string propName, IEnumerable<MemberInfo> members)
        {
            if (members == null)
                members = new MemberInfo[] { };
            return new XElement(propName, 
                                from member in members
                                select GenerateXmlFromProperty(member.GetType(), "Info", member));
        }

        object GenerateXmlFromBindingList(string propName, IEnumerable<MemberBinding> bindings)
        {
            if (bindings == null)
                bindings = new MemberBinding[] { };
            return new XElement(propName, 
                                from binding in bindings
                                select GenerateXmlFromBinding(binding));
        }

        object GenerateXmlFromBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return GenerateXmlFromAssignment(binding as MemberAssignment);
                case MemberBindingType.ListBinding:
                    return GenerateXmlFromListBinding(binding as MemberListBinding);
                case MemberBindingType.MemberBinding:
                    return GenerateXmlFromMemberBinding(binding as MemberMemberBinding);
                default:
                    throw new NotSupportedException(string.Format("Binding type {0} not supported.", binding.BindingType));
            }
        }

        object GenerateXmlFromMemberBinding(MemberMemberBinding memberMemberBinding)
        {
            return new XElement("MemberMemberBinding", 
                                GenerateXmlFromProperty(memberMemberBinding.Member.GetType(), "Member", memberMemberBinding.Member), 
                                GenerateXmlFromBindingList("Bindings", memberMemberBinding.Bindings));
        }

        object GenerateXmlFromListBinding(MemberListBinding memberListBinding)
        {
            return new XElement("MemberListBinding", 
                                GenerateXmlFromProperty(memberListBinding.Member.GetType(), "Member", memberListBinding.Member), 
                                GenerateXmlFromProperty(memberListBinding.Initializers.GetType(), "Initializers", memberListBinding.Initializers));
        }

        object GenerateXmlFromAssignment(MemberAssignment memberAssignment)
        {
            return new XElement("MemberAssignment", 
                                GenerateXmlFromProperty(memberAssignment.Member.GetType(), "Member", memberAssignment.Member), 
                                GenerateXmlFromProperty(memberAssignment.Expression.GetType(), "Expression", memberAssignment.Expression));
        }

        XElement GenerateXmlFromExpression(string propName, Expression e)
        {
            return new XElement(propName, GenerateXmlFromExpressionCore(e));
        }

        object GenerateXmlFromType(string propName, Type type)
        {
            return new XElement(propName, GenerateXmlFromTypeCore(type));
        }

        XElement GenerateXmlFromTypeCore(Type type)
        {
            // vsadov: add detection of VB anon types
            if (type.Name.StartsWith("<>f__") || type.Name.StartsWith("VB$AnonymousType"))
            {
                return new XElement("AnonymousType", 
                                    new XAttribute("Name", type.FullName), 
                                    from property in type.GetProperties()
                                    select new XElement("Property", 
                                                        new XAttribute("Name", property.Name), 
                                                        GenerateXmlFromTypeCore(property.PropertyType)), 
                                    new XElement("Constructor", 
                                                 from parameter in type.GetConstructors().First().GetParameters()
                                                 select new XElement("Parameter", 
                                                                     new XAttribute("Name", parameter.Name), 
                                                                     GenerateXmlFromTypeCore(parameter.ParameterType))
                                            ));
            }
            else
            {
                // vsadov: GetGenericArguments returns args for nongeneric types 
                // like arrays no need to save them.
                if (type.IsGenericType)
                {
                    return new XElement("Type", 
                                        new XAttribute("Name", type.GetGenericTypeDefinition().FullName), 
                                        from genArgType in type.GetGenericArguments()
                                        select GenerateXmlFromTypeCore(genArgType));
                }
                else
                    return new XElement("Type", new XAttribute("Name", type.FullName));
            }
        }

        object GenerateXmlFromPrimitive(string propName, object value)
        {
            return new XAttribute(propName, value ?? string.Empty);
        }

        object GenerateXmlFromMethodInfo(string propName, MethodInfo methodInfo)
        {
            if (methodInfo == null)
                return new XElement(propName);
            return new XElement(propName, 
                                new XAttribute("MemberType", methodInfo.MemberType), 
                                new XAttribute("MethodName", methodInfo.Name), 
                                GenerateXmlFromType("DeclaringType", methodInfo.DeclaringType), 
                                new XElement("Parameters", 
                                             from param in methodInfo.GetParameters()
                                             select GenerateXmlFromType("Type", param.ParameterType)), 
                                new XElement("GenericArgTypes", 
                                             from argType in methodInfo.GetGenericArguments()
                                             select GenerateXmlFromType("Type", argType)));
        }

        object GenerateXmlFromPropertyInfo(string propName, PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                return new XElement(propName);
            return new XElement(propName, 
                                new XAttribute("MemberType", propertyInfo.MemberType), 
                                new XAttribute("PropertyName", propertyInfo.Name), 
                                GenerateXmlFromType("DeclaringType", propertyInfo.DeclaringType), 
                                new XElement("IndexParameters", 
                                             from param in propertyInfo.GetIndexParameters()
                                             select GenerateXmlFromType("Type", param.ParameterType)));
        }

        object GenerateXmlFromFieldInfo(string propName, FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                return new XElement(propName);
            return new XElement(propName, 
                                new XAttribute("MemberType", fieldInfo.MemberType), 
                                new XAttribute("FieldName", fieldInfo.Name), 
                                GenerateXmlFromType("DeclaringType", fieldInfo.DeclaringType));
        }

        object GenerateXmlFromConstructorInfo(string propName, ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
                return new XElement(propName);
            return new XElement(propName, 
                                new XAttribute("MemberType", constructorInfo.MemberType), 
                                new XAttribute("MethodName", constructorInfo.Name), 
                                GenerateXmlFromType("DeclaringType", constructorInfo.DeclaringType), 
                                new XElement("Parameters", 
                                             from param in constructorInfo.GetParameters()
                                             select new XElement("Parameter", 
                                                                 new XAttribute("Name", param.Name), 
                                                                 GenerateXmlFromType("Type", param.ParameterType))));
        }

        /*
         * DESERIALIZATION 
         */
        Expression ParseExpressionFromXml(XElement xml)
        {
            if (xml.IsEmpty)
                return null;

            return ParseExpressionFromXmlNonNull(xml.Elements().First());
        }

        Expression ParseExpressionFromXmlNonNull(XElement xml)
        {
            var expression = ApplyCustomDeserializers(xml);

            if (expression != null)
                return expression;
            switch (xml.Name.LocalName)
            {
                case "BinaryExpression":
                case "SimpleBinaryExpression":
                case "LogicalBinaryExpression":
                case "MethodBinaryExpression":
                    return ParseBinaryExpresssionFromXml(xml);
                case "ConstantExpression":
                case "TypedConstantExpression":
                    return ParseConstatExpressionFromXml(xml);
                case "ParameterExpression":
                case "PrimitiveParameterExpression_x0060_1":
                case "PrimitiveParameterExpressionx00601":
                case "TypedParameterExpression":
                    return ParseParameterExpressionFromXml(xml);
                case "LambdaExpression":
                    return ParseLambdaExpressionFromXml(xml);
                case "MethodCallExpression":
                case "MethodCallExpressionN":
                case "InstanceMethodCallExpressionN":
                    return ParseMethodCallExpressionFromXml(xml);
                case "UnaryExpression":
                    return ParseUnaryExpressionFromXml(xml);
                case "MemberExpression":
                case "PropertyExpression":
                case "FieldExpression":
                    return ParseMemberExpressionFromXml(xml);
                case "NewExpression":
                    return ParseNewExpressionFromXml(xml);
                case "ListInitExpression":
                    return ParseListInitExpressionFromXml(xml);
                case "MemberInitExpression":
                    return ParseMemberInitExpressionFromXml(xml);
                case "ConditionalExpression":
                case "FullConditionalExpression":
                    return ParseConditionalExpressionFromXml(xml);
                case "NewArrayExpression":
                case "NewArrayInitExpression":
                case "NewArrayBoundsExpression":
                    return ParseNewArrayExpressionFromXml(xml);
                case "TypeBinaryExpression":
                    return ParseTypeBinaryExpressionFromXml(xml);
                case "InvocationExpression":
                    return ParseInvocationExpressionFromXml(xml);
                default:
                    throw new NotSupportedException(xml.Name.LocalName);
            }
        }

        Expression ApplyCustomDeserializers(XElement xml)
        {
            foreach (var converter in Converters)
            {
                var result = converter.Deserialize(xml);
                if (result != null)
                    return result;
            }

            return null;
        }

        Expression ParseInvocationExpressionFromXml(XElement xml)
        {
            var expression = ParseExpressionFromXml(xml.Element("Expression"));
            var arguments = ParseExpressionListFromXml<Expression>(xml, "Arguments");
            return Expression.Invoke(expression, arguments);
        }

        Expression ParseTypeBinaryExpressionFromXml(XElement xml)
        {
            var expression = ParseExpressionFromXml(xml.Element("Expression"));
            var typeOperand = ParseTypeFromXml(xml.Element("TypeOperand"));
            return Expression.TypeIs(expression, typeOperand);
        }

        Expression ParseNewArrayExpressionFromXml(XElement xml)
        {
            var type = ParseTypeFromXml(xml.Element("Type"));
            if (!type.IsArray)
                throw new Exception("Expected array type");
            var elemType = type.GetElementType();
            var expressions = ParseExpressionListFromXml<Expression>(xml, "Expressions");
            switch (xml.Attribute("NodeType").Value)
            {
                case "NewArrayInit":
                    return Expression.NewArrayInit(elemType, expressions);
                case "NewArrayBounds":
                    return Expression.NewArrayBounds(elemType, expressions);
                default:
                    throw new Exception("Expected NewArrayInit or NewArrayBounds");
            }
        }

        Expression ParseConditionalExpressionFromXml(XElement xml)
        {
            var test = ParseExpressionFromXml(xml.Element("Test"));
            var ifTrue = ParseExpressionFromXml(xml.Element("IfTrue"));
            var ifFalse = ParseExpressionFromXml(xml.Element("IfFalse"));
            return Expression.Condition(test, ifTrue, ifFalse);
        }

        Expression ParseMemberInitExpressionFromXml(XElement xml)
        {
            var newExpression = ParseNewExpressionFromXml(xml.Element("NewExpression").Element("NewExpression")) as NewExpression;
            var bindings = ParseBindingListFromXml(xml, "Bindings").ToArray();
            return Expression.MemberInit(newExpression, bindings);
        }

        Expression ParseListInitExpressionFromXml(XElement xml)
        {
            var newExpression = ParseExpressionFromXml(xml.Element("NewExpression")) as NewExpression;
            if (newExpression == null)
                throw new Exception("Expceted a NewExpression");
            var initializers = ParseElementInitListFromXml(xml, "Initializers").ToArray();
            return Expression.ListInit(newExpression, initializers);
        }

        Expression ParseNewExpressionFromXml(XElement xml)
        {
            var constructor = ParseConstructorInfoFromXml(xml.Element("Constructor"));
            var arguments = ParseExpressionListFromXml<Expression>(xml, "Arguments").ToArray();
            var members = ParseMemberInfoListFromXml<MemberInfo>(xml, "Members").ToArray();
            if (members.Length == 0)
                return Expression.New(constructor, arguments);
            return Expression.New(constructor, arguments, members);
        }

        Expression ParseMemberExpressionFromXml(XElement xml)
        {
            var expression = ParseExpressionFromXml(xml.Element("Expression"));
            var member = ParseMemberInfoFromXml(xml.Element("Member"));
            return Expression.MakeMemberAccess(expression, member);
        }

        MemberInfo ParseMemberInfoFromXml(XElement xml)
        {
            var memberType = (MemberTypes)ParseConstantFromAttribute<MemberTypes>(xml, "MemberType");
            switch (memberType)
            {
                case MemberTypes.Field:
                    return ParseFieldInfoFromXml(xml);
                case MemberTypes.Property:
                    return ParsePropertyInfoFromXml(xml);
                case MemberTypes.Method:
                    return ParseMethodInfoFromXml(xml);
                case MemberTypes.Constructor:
                    return ParseConstructorInfoFromXml(xml);
                case MemberTypes.Custom:
                case MemberTypes.Event:
                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                default:
                    throw new NotSupportedException(string.Format("MEmberType {0} not supported", memberType));
            }
        }

        MemberInfo ParseFieldInfoFromXml(XElement xml)
        {
            string fieldName = (string)ParseConstantFromAttribute<string>(xml, "FieldName");
            var declaringType = ParseTypeFromXml(xml.Element("DeclaringType"));
            return declaringType.GetField(fieldName);
        }

        MemberInfo ParsePropertyInfoFromXml(XElement xml)
        {
            string propertyName = (string)ParseConstantFromAttribute<string>(xml, "PropertyName");
            var declaringType = ParseTypeFromXml(xml.Element("DeclaringType"));
            var ps = from paramXml in xml.Element("IndexParameters").Elements()
                     select ParseTypeFromXml(paramXml);

            // return declaringType.GetProperty(propertyName, typeof(Type), ps.ToArray());
            return declaringType.GetProperty(propertyName, ps.ToArray());
        }

        Expression ParseUnaryExpressionFromXml(XElement xml)
        {
            var operand = ParseExpressionFromXml(xml.Element("Operand"));
            var method = ParseMethodInfoFromXml(xml.Element("Method"));
            bool isLifted = (bool)ParseConstantFromAttribute<bool>(xml, "IsLifted");
            bool isLiftedToNull = (bool)ParseConstantFromAttribute<bool>(xml, "IsLiftedToNull");
            var expressionType = (ExpressionType)ParseConstantFromAttribute<ExpressionType>(xml, "NodeType");
            var type = ParseTypeFromXml(xml.Element("Type"));

            // TODO: Why can't we use IsLifted and IsLiftedToNull here?  
            // May need to special case a nodeType if it needs them.
            return Expression.MakeUnary(expressionType, operand, type, method);
        }

        Expression ParseMethodCallExpressionFromXml(XElement xml)
        {
            var instance = ParseExpressionFromXml(xml.Element("Object"));
            var method = ParseMethodInfoFromXml(xml.Element("Method"));
            var arguments = ParseExpressionListFromXml<Expression>(xml, "Arguments").ToArray();
            return Expression.Call(instance, method, arguments);
        }

        Expression ParseLambdaExpressionFromXml(XElement xml)
        {
            var body = ParseExpressionFromXml(xml.Element("Body"));
            var parameters = ParseExpressionListFromXml<ParameterExpression>(xml, "Parameters");
            var type = ParseTypeFromXml(xml.Element("Type"));

            // We may need to 
            // var lambdaExpressionReturnType = type.GetMethod("Invoke").ReturnType;
            // if (lambdaExpressionReturnType.IsArray)
            // {

            // type = typeof(IEnumerable<>).MakeGenericType(type.GetElementType());
            // }
            return Expression.Lambda(type, body, parameters);
        }

        IEnumerable<T> ParseExpressionListFromXml<T>(XElement xml, string elemName) where T : Expression
        {
            return from tXml in xml.Element(elemName).Elements()
                   select (T)ParseExpressionFromXmlNonNull(tXml);
        }

        IEnumerable<T> ParseMemberInfoListFromXml<T>(XElement xml, string elemName) where T : MemberInfo
        {
            return from tXml in xml.Element(elemName).Elements()
                   select (T)ParseMemberInfoFromXml(tXml);
        }

        IEnumerable<ElementInit> ParseElementInitListFromXml(XElement xml, string elemName)
        {
            return from tXml in xml.Element(elemName).Elements()
                   select ParseElementInitFromXml(tXml);
        }

        ElementInit ParseElementInitFromXml(XElement xml)
        {
            var addMethod = ParseMethodInfoFromXml(xml.Element("AddMethod"));
            var arguments = ParseExpressionListFromXml<Expression>(xml, "Arguments");
            return Expression.ElementInit(addMethod, arguments);
        }

        IEnumerable<MemberBinding> ParseBindingListFromXml(XElement xml, string elemName)
        {
            return from tXml in xml.Element(elemName).Elements()
                   select ParseBindingFromXml(tXml);
        }

        MemberBinding ParseBindingFromXml(XElement tXml)
        {
            var member = ParseMemberInfoFromXml(tXml.Element("Member"));
            switch (tXml.Name.LocalName)
            {
                case "MemberAssignment":
                    var expression = ParseExpressionFromXml(tXml.Element("Expression"));
                    return Expression.Bind(member, expression);
                case "MemberMemberBinding":
                    var bindings = ParseBindingListFromXml(tXml, "Bindings");
                    return Expression.MemberBind(member, bindings);
                case "MemberListBinding":
                    var initializers = ParseElementInitListFromXml(tXml, "Initializers");
                    return Expression.ListBind(member, initializers);
            }

            throw new NotImplementedException();
        }

        Expression ParseParameterExpressionFromXml(XElement xml)
        {
            var type = ParseTypeFromXml(xml.Element("Type"));
            string name = (string)ParseConstantFromAttribute<string>(xml, "Name");

            // vs: hack
            string id = name + type.FullName;
            if (!this.parameters.ContainsKey(id))
                this.parameters.Add(id, Expression.Parameter(type, name));
            return this.parameters[id];
        }

        Expression ParseConstatExpressionFromXml(XElement xml)
        {
            var type = ParseTypeFromXml(xml.Element("Type"));
            return Expression.Constant(ParseConstantFromElement(xml, "Value", type), type);
        }

        Type ParseTypeFromXml(XElement xml)
        {
            Debug.Assert(xml.Elements().Count() == 1);
            return ParseTypeFromXmlCore(xml.Elements().First());
        }

        Type ParseTypeFromXmlCore(XElement xml)
        {
            switch (xml.Name.ToString())
            {
                case "Type":
                    return ParseNormalTypeFromXmlCore(xml);
                case "AnonymousType":
                    return ParseAnonymousTypeFromXmlCore(xml);
                default:
                    throw new ArgumentException("Expected 'Type' or 'AnonymousType'");
            }
        }

        Type ParseNormalTypeFromXmlCore(XElement xml)
        {
            if (!xml.HasElements)
                return this.resolver.GetType(xml.Attribute("Name").Value);

            var genericArgumentTypes = from genArgXml in xml.Elements()
                                       select ParseTypeFromXmlCore(genArgXml);
            return this.resolver.GetType(xml.Attribute("Name").Value, genericArgumentTypes);
        }

        Type ParseAnonymousTypeFromXmlCore(XElement xElement)
        {
            string name = xElement.Attribute("Name").Value;
            var properties = from propXml in xElement.Elements("Property")
                             select new ExpressionSerializationTypeResolver.NameTypePair
                                        {
                                                Name = propXml.Attribute("Name").Value, 
                                                Type = ParseTypeFromXml(propXml)
                                        };
            var ctr_params = from propXml in xElement.Elements("Constructor").Elements("Parameter")
                             select new ExpressionSerializationTypeResolver.NameTypePair
                                        {
                                                Name = propXml.Attribute("Name").Value, 
                                                Type = ParseTypeFromXml(propXml)
                                        };

            return this.resolver.GetOrCreateAnonymousTypeFor(name, properties.ToArray(), ctr_params.ToArray());
        }

        Expression ParseBinaryExpresssionFromXml(XElement xml)
        {
            var expressionType = (ExpressionType)ParseConstantFromAttribute<ExpressionType>(xml, "NodeType");

            var left = ParseExpressionFromXml(xml.Element("Left"));
            var right = ParseExpressionFromXml(xml.Element("Right"));
            bool isLifted = (bool)ParseConstantFromAttribute<bool>(xml, "IsLifted");
            bool isLiftedToNull = (bool)ParseConstantFromAttribute<bool>(xml, "IsLiftedToNull");
            var type = ParseTypeFromXml(xml.Element("Type"));
            var method = ParseMethodInfoFromXml(xml.Element("Method"));
            var conversion = ParseExpressionFromXml(xml.Element("Conversion")) as LambdaExpression;
            if (expressionType == ExpressionType.Coalesce)
                return Expression.Coalesce(left, right, conversion);
            return Expression.MakeBinary(expressionType, left, right, isLiftedToNull, method);
        }

        MethodInfo ParseMethodInfoFromXml(XElement xml)
        {
            if (xml.IsEmpty)
                return null;
            string name = (string)ParseConstantFromAttribute<string>(xml, "MethodName");
            var declaringType = ParseTypeFromXml(xml.Element("DeclaringType"));
            var ps = from paramXml in xml.Element("Parameters").Elements()
                     select ParseTypeFromXml(paramXml);
            var genArgs = from argXml in xml.Element("GenericArgTypes").Elements()
                          select ParseTypeFromXml(argXml);
            return this.resolver.GetMethod(declaringType, name, ps.ToArray(), genArgs.ToArray());
        }

        ConstructorInfo ParseConstructorInfoFromXml(XElement xml)
        {
            if (xml.IsEmpty)
                return null;
            var declaringType = ParseTypeFromXml(xml.Element("DeclaringType"));
            var ps = from paramXml in xml.Element("Parameters").Elements()
                     select ParseParameterFromXml(paramXml);
            var ci = declaringType.GetConstructor(ps.ToArray());
            return ci;
        }

        Type ParseParameterFromXml(XElement xml)
        {
            string name = (string)ParseConstantFromAttribute<string>(xml, "Name");
            var type = ParseTypeFromXml(xml.Element("Type"));
            return type;
        }

        object ParseConstantFromAttribute<T>(XElement xml, string attrName)
        {
            string objectStringValue = xml.Attribute(attrName).Value;
            if (typeof(Type).IsAssignableFrom(typeof(T)))
                throw new Exception("We should never be encoding Types in attributes now.");
            if (typeof(Enum).IsAssignableFrom(typeof(T)))
                return Enum.Parse(typeof(T), objectStringValue, true);
            return Convert.ChangeType(objectStringValue, typeof(T), CultureInfo.CurrentCulture);
        }

        object ParseConstantFromElement(XElement xml, string elemName, Type type)
        {
            string objectStringValue = xml.Element(elemName).Value;
            if (typeof(Type).IsAssignableFrom(type))
                return ParseTypeFromXml(xml.Element("Value"));
            if (typeof(Enum).IsAssignableFrom(type))
                return Enum.Parse(type, objectStringValue, true);
            return Convert.ChangeType(objectStringValue, type, CultureInfo.CurrentCulture);
        }

        ////ncrunch: no coverage end    
    }
}