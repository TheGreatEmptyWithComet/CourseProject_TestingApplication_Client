﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestingClientApp.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.6.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Quiz data\\\\Users.txt")]
        public string UsersDataFileName {
            get {
                return ((string)(this["UsersDataFileName"]));
            }
            set {
                this["UsersDataFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Quiz data\\\\Statistic.txt")]
        public string StatDataFileName {
            get {
                return ((string)(this["StatDataFileName"]));
            }
            set {
                this["StatDataFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("20")]
        public int QuizQuestionAmount {
            get {
                return ((int)(this["QuizQuestionAmount"]));
            }
            set {
                this["QuizQuestionAmount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("20")]
        public int TopResultAmount {
            get {
                return ((int)(this["TopResultAmount"]));
            }
            set {
                this["TopResultAmount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Quiz data\\\\Questions\\\\Text")]
        public string QuestionsTextDirectory {
            get {
                return ((string)(this["QuestionsTextDirectory"]));
            }
            set {
                this["QuestionsTextDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Custom")]
        public string CustomCategoryName {
            get {
                return ((string)(this["CustomCategoryName"]));
            }
            set {
                this["CustomCategoryName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int TotalQuizScore {
            get {
                return ((int)(this["TotalQuizScore"]));
            }
            set {
                this["TotalQuizScore"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
        public int TimeForQuiz_minuts {
            get {
                return ((int)(this["TimeForQuiz_minuts"]));
            }
            set {
                this["TimeForQuiz_minuts"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("180")]
        public string TimeForQuiz_seconds {
            get {
                return ((string)(this["TimeForQuiz_seconds"]));
            }
            set {
                this["TimeForQuiz_seconds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Quiz data\\\\Questions\\\\Images")]
        public string CategoryImagesDirectory {
            get {
                return ((string)(this["CategoryImagesDirectory"]));
            }
            set {
                this["CategoryImagesDirectory"] = value;
            }
        }
    }
}
