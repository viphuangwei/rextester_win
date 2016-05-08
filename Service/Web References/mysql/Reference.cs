﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Service.mysql {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ServiceSoap", Namespace="http://rextester.com/")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback DoWorkOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetCurrentUserOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service() {
            this.Url = global::Service.Properties.Settings.Default.Service_mysql_Service;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event DoWorkCompletedEventHandler DoWorkCompleted;
        
        /// <remarks/>
        public event GetCurrentUserCompletedEventHandler GetCurrentUserCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://rextester.com/DoWork", RequestNamespace="http://rextester.com/", ResponseNamespace="http://rextester.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Result DoWork(string Program, string Input, Languages Language, string user, string pass, string compiler_args, bool bytes, bool programCompressed, bool inputCompressed) {
            object[] results = this.Invoke("DoWork", new object[] {
                        Program,
                        Input,
                        Language,
                        user,
                        pass,
                        compiler_args,
                        bytes,
                        programCompressed,
                        inputCompressed});
            return ((Result)(results[0]));
        }
        
        /// <remarks/>
        public void DoWorkAsync(string Program, string Input, Languages Language, string user, string pass, string compiler_args, bool bytes, bool programCompressed, bool inputCompressed) {
            this.DoWorkAsync(Program, Input, Language, user, pass, compiler_args, bytes, programCompressed, inputCompressed, null);
        }
        
        /// <remarks/>
        public void DoWorkAsync(string Program, string Input, Languages Language, string user, string pass, string compiler_args, bool bytes, bool programCompressed, bool inputCompressed, object userState) {
            if ((this.DoWorkOperationCompleted == null)) {
                this.DoWorkOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDoWorkOperationCompleted);
            }
            this.InvokeAsync("DoWork", new object[] {
                        Program,
                        Input,
                        Language,
                        user,
                        pass,
                        compiler_args,
                        bytes,
                        programCompressed,
                        inputCompressed}, this.DoWorkOperationCompleted, userState);
        }
        
        private void OnDoWorkOperationCompleted(object arg) {
            if ((this.DoWorkCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DoWorkCompleted(this, new DoWorkCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://rextester.com/GetCurrentUser", RequestNamespace="http://rextester.com/", ResponseNamespace="http://rextester.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetCurrentUser() {
            object[] results = this.Invoke("GetCurrentUser", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetCurrentUserAsync() {
            this.GetCurrentUserAsync(null);
        }
        
        /// <remarks/>
        public void GetCurrentUserAsync(object userState) {
            if ((this.GetCurrentUserOperationCompleted == null)) {
                this.GetCurrentUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCurrentUserOperationCompleted);
            }
            this.InvokeAsync("GetCurrentUser", new object[0], this.GetCurrentUserOperationCompleted, userState);
        }
        
        private void OnGetCurrentUserOperationCompleted(object arg) {
            if ((this.GetCurrentUserCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCurrentUserCompleted(this, new GetCurrentUserCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://rextester.com/")]
    public enum Languages {
        
        /// <remarks/>
        VCPP,
        
        /// <remarks/>
        VC,
        
        /// <remarks/>
        MySql,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://rextester.com/")]
    public partial class Result {
        
        private string errorsField;
        
        private byte[] errors_BytesField;
        
        private string warningsField;
        
        private byte[] warnings_BytesField;
        
        private string outputField;
        
        private bool isOutputCompressedField;
        
        private byte[] output_BytesField;
        
        private string statsField;
        
        private string exit_StatusField;
        
        private System.Nullable<int> exit_CodeField;
        
        private string system_ErrorField;
        
        private byte[][] filesField;
        
        /// <remarks/>
        public string Errors {
            get {
                return this.errorsField;
            }
            set {
                this.errorsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] Errors_Bytes {
            get {
                return this.errors_BytesField;
            }
            set {
                this.errors_BytesField = value;
            }
        }
        
        /// <remarks/>
        public string Warnings {
            get {
                return this.warningsField;
            }
            set {
                this.warningsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] Warnings_Bytes {
            get {
                return this.warnings_BytesField;
            }
            set {
                this.warnings_BytesField = value;
            }
        }
        
        /// <remarks/>
        public string Output {
            get {
                return this.outputField;
            }
            set {
                this.outputField = value;
            }
        }
        
        /// <remarks/>
        public bool IsOutputCompressed {
            get {
                return this.isOutputCompressedField;
            }
            set {
                this.isOutputCompressedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] Output_Bytes {
            get {
                return this.output_BytesField;
            }
            set {
                this.output_BytesField = value;
            }
        }
        
        /// <remarks/>
        public string Stats {
            get {
                return this.statsField;
            }
            set {
                this.statsField = value;
            }
        }
        
        /// <remarks/>
        public string Exit_Status {
            get {
                return this.exit_StatusField;
            }
            set {
                this.exit_StatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> Exit_Code {
            get {
                return this.exit_CodeField;
            }
            set {
                this.exit_CodeField = value;
            }
        }
        
        /// <remarks/>
        public string System_Error {
            get {
                return this.system_ErrorField;
            }
            set {
                this.system_ErrorField = value;
            }
        }
        
        /// <remarks/>
        public byte[][] Files {
            get {
                return this.filesField;
            }
            set {
                this.filesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void DoWorkCompletedEventHandler(object sender, DoWorkCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DoWorkCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DoWorkCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Result Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Result)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void GetCurrentUserCompletedEventHandler(object sender, GetCurrentUserCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCurrentUserCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetCurrentUserCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591