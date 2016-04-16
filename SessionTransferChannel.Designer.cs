namespace SessionsStore
{
    partial class SessionTransferChannel
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                this.Close();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.RunChannelIdleTimeout = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.RunChannelIdleTimeout)).BeginInit();
            // 
            // RunChannelIdleTimeout
            // 
            this.RunChannelIdleTimeout.Interval = 1D;
            this.RunChannelIdleTimeout.Elapsed += new System.Timers.ElapsedEventHandler(this.Elapsed);
            ((System.ComponentModel.ISupportInitialize)(this.RunChannelIdleTimeout)).EndInit();

        }

        #endregion

        private System.Timers.Timer RunChannelIdleTimeout;

    }
}
