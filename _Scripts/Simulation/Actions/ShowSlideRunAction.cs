using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    public class ShowSlideRunAction : ActionBase
    {

        public string startValue;
        public string endValue;

        /// <summary>
        /// 是否转完100才完成Action
        /// </summary>
        private bool completed;
        private Archite_SlideRun slide;

        public ShowSlideRunAction(Archite_SlideRun _slide,string _startValue, string _endValue, bool _isCompleted = false)
        {
            slide = _slide;
            startValue = _startValue;
            endValue = _endValue;
            completed = _isCompleted;           
        }

        public override void Execute()
        {
            if (slide != null)
            {
                slide.gameObject.SetActive(true);
                if (completed)
                {
                    slide.SetData(startValue, endValue, ProgressPanel_OnCompleted);
                }
                else
                {
                    ProgressPanel_OnCompleted();
                    slide.SetData(startValue, endValue);
                }

            }
            else
            {
                Error(new Exception("Archite_SlideRun is null"));
            }
        }
        private void ProgressPanel_OnCompleted()
        {
            Completed();
        }
    }
}
