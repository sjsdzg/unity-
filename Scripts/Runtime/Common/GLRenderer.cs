using XFramework.Core;

namespace XFramework.Runtime
{
    interface IGL
    {
        void OnDrawFrame();
    }

    public class GLRenderer : Singleton<GLRenderer>
    {

        protected override void Init()
        {
            base.Init();
        }

        public void Draw()
        {
            
        }
    }
}