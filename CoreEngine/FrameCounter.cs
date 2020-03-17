using System;

namespace CoreEngine
{
    public class FrameCounter
    {
        public int frames;
        private int lastSecond;

        public Action<int> Callback { get; set; }

        public void Update()
        {
            DateTime now = DateTime.Now;
            if (now.Second != this.lastSecond)
            {
                this.lastSecond = now.Second;
                this.Callback?.Invoke(this.frames);
                this.frames = 1;
            }
            else
            {
                this.frames++;
            }
        }
    }
}
