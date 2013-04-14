namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    // ReSharper disable UnusedMember.Global
    [Flags]
    public enum ModernizrSupport
    {
        js = 0x00000001, 

        No_Touch = 0x00000002, 

        PostMessage = 0x00000004, 

        history = 0x00000008, 

        Multiplebgs = 0x00000010, 

        BoxShadow = 0x00000020, 

        Opacity = 0x00000040, 

        CssAnimations = 0x00000080, 

        CssColumns = 0x000001000, 

        CssGradients = 0x00002000, 

        CssTransforms = 0x00004000, 

        CssTransitions = 0x00008000, 

        FontFace = 0x00010000, 

        LocalStorage = 0x00020000, 

        SessionStorage = 0x00040000, 

        Svg = 0x00080000, 

        InlineSvg = 0x00100000, 

        No_BlobBuilder = 0x00200000, 

        blob = 0x00400000, 

        BlobUrls = 0x00800000, 

        No_Download = 0x01000000, 

        FormData = 0x02000000, 

        wf_proximanova1proximanova2_n4_active = 0x04000000, 

        wf_proximanova1proximanova2_i4_active = 0x08000000, 

        wf_proximanova1proximanova2_n7_active = 0x01600000, 

        wf_proximanova1proximanova2_i7_active = 0x03200000, 

        wf_proximanovacondensed1proximanovacondensed2_n6_active = 0x06400000, 

        wf_athelas1athelas2_n4_active = 0x06400000, 

        wf_active = 0x12800000
    }

    // ReSharper restore UnusedMember.Global
}