﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.18444
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HIA_client_lourd.Properties {
    
    
    [System.Runtime.CompilerServices.CompilerGenerated()]
    [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [System.Configuration.ApplicationScopedSetting()]
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Configuration.SpecialSetting(System.Configuration.SpecialSetting.ConnectionString)]
        [System.Configuration.DefaultSettingValue("Data Source=(LocalDB)\\v11.0;AttachDbFilename=|DataDirectory|\\dbHIDIV.mdf;Integrat" +
            "ed Security=True")]
        public string dbHIDIVConnectionString {
            get {
                return ((string)(this["dbHIDIVConnectionString"]));
            }
        }
        
        [System.Configuration.ApplicationScopedSetting()]
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Configuration.SpecialSetting(System.Configuration.SpecialSetting.ConnectionString)]
        [System.Configuration.DefaultSettingValue("Data Source=(LocalDB)\\v11.0;AttachDbFilename=|DataDirectory|\\db.mdf;Integrated Se" +
            "curity=True")]
        public string dbConnectionString {
            get {
                return ((string)(this["dbConnectionString"]));
            }
        }
    }
}
