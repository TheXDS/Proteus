﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TheXDS.Proteus.Resources.Strings {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class SqlCommon {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SqlCommon() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TheXDS.Proteus.Resources.Strings.SqlCommon", typeof(SqlCommon).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Connection established successfully..
        /// </summary>
        public static string ConnSuccess {
            get {
                return ResourceManager.GetString("ConnSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Could not connect.
        /// </summary>
        public static string CouldNotConnect {
            get {
                return ResourceManager.GetString("CouldNotConnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Invalid connection string.
        /// </summary>
        public static string InvalidConnStr {
            get {
                return ResourceManager.GetString("InvalidConnStr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a You must supply a valid SQL connection string to continue..
        /// </summary>
        public static string InvalidConnStrMsg {
            get {
                return ResourceManager.GetString("InvalidConnStrMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a SQL connection.
        /// </summary>
        public static string SQLConn {
            get {
                return ResourceManager.GetString("SQLConn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Testing connection.
        /// </summary>
        public static string TestingConnection {
            get {
                return ResourceManager.GetString("TestingConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Trying to connect using &quot;{0}&quot;....
        /// </summary>
        public static string TryingConn {
            get {
                return ResourceManager.GetString("TryingConn", resourceCulture);
            }
        }
    }
}
