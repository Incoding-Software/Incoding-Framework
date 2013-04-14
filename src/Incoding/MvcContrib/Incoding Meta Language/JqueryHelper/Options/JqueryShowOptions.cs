namespace Incoding.MvcContrib
{
    public class JqueryShowOptions
    {
        ////ncrunch: no coverage start
        #region Static Fields

        public static readonly JqueryShowOptions Fast = new JqueryShowOptions
                                                            {
                                                                    Effect = JuqeryEffect.Slide, 
                                                                    Easing = JqueryEasing.InExpo, 
                                                                    Duration = 50
                                                            };

        public static readonly JqueryShowOptions Middle = new JqueryShowOptions
                                                              {
                                                                      Effect = JuqeryEffect.Slide, 
                                                                      Easing = JqueryEasing.InExpo, 
                                                                      Duration = 500
                                                              };

        public static readonly JqueryShowOptions Slow = new JqueryShowOptions
                                                            {
                                                                    Effect = JuqeryEffect.Slide, 
                                                                    Easing = JqueryEasing.InExpo, 
                                                                    Duration = 2000
                                                            };

        #endregion

        ////ncrunch: no coverage end
        #region Properties

        public JuqeryEffect Effect { get; set; }

        public JqueryEasing Easing { get; set; }

        public int Duration { get; set; }

        #endregion
    }
}