namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;

    #endregion

    public class IncodingMetaCallbackStoreApiDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        #endregion

        #region Constructors

        public IncodingMetaCallbackStoreApiDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

        #region Properties

        public IncodingMetaCallbackStoreDsl Hash { get { return new IncodingMetaCallbackStoreDsl("hash", this.plugIn); } }

        public IncodingMetaCallbackStoreDsl QueryString { get { return new IncodingMetaCallbackStoreDsl("queryString", this.plugIn); } }

        #endregion

        #region Nested classes

        public class IncodingMetaCallbackStoreDsl
        {
            #region Fields

            readonly string type;

            readonly IIncodingMetaLanguagePlugInDsl plugIn;

            #endregion

            #region Constructors

            public IncodingMetaCallbackStoreDsl(string type, IIncodingMetaLanguagePlugInDsl plugIn)
            {
                this.type = type;
                this.plugIn = plugIn;
            }

            #endregion

            #region Api Methods

            public IExecutableSetting Insert(string prefix = "root")
            {
                return this.plugIn.Registry(new ExecutableStoreInsert(this.type, false, prefix));
            }

            public IExecutableSetting Fetch(string prefix = "root")
            {
                return this.plugIn.Registry(new ExecutableStoreFetch(this.type, prefix));
            }

            public IExecutableSetting Set(string prefix = "root")
            {
                return this.plugIn.Registry(new ExecutableStoreInsert(this.type, true, prefix));
            }

            public IExecutableSetting Manipulate(Action<IncodingMetaCallbackStoreManipulateDsl> configuration)
            {
                var manipulate = new IncodingMetaCallbackStoreManipulateDsl();
                configuration(manipulate);
                return this.plugIn.Registry(new ExecutableStoreManipulate(this.type, manipulate.methods));
            }

            #endregion

            #region Nested classes

            public class IncodingMetaCallbackStoreManipulateDsl
            {
                #region Fields

                internal readonly List<object> methods = new List<object>();

                #endregion

                #region Api Methods

                public IncodingMetaCallbackStoreManipulateDsl Remove(string key, string prefix = "root")
                {
                    this.methods.Add(new { verb = "remove", key, prefix });
                    return this;
                }

                public IncodingMetaCallbackStoreManipulateDsl Set(string key, Selector value, string prefix = "root")
                {
                    this.methods.Add(new { verb = "set", key, value = value.ToString(), prefix });
                    return this;
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}