﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AoC.Puzzles2016.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AoC.Puzzles2016.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///#DEFINITIONS
        ///
        ///Id              = &quot;an identifier&quot; | [_A-Za-z][_A-Za-z0-9]*
        ///String          = &quot;a string&quot; | &apos;[.]*&apos;
        ///Integer         = &quot;an integer&quot; | (\+|-)?[0-9]+
        ///Real            = &quot;a real number&quot; | (\+|-)?[0-9]+\.[0-9]+
        ///
        ///#GRAMMAR
        ///
        ///program         = line moreLines
        ///
        ///moreLines       =
        ///                | line moreLines
        ///
        ///line            = &quot;Monkey&quot; Integer &quot;:&quot; s_monkey
        ///                | &quot;Starting&quot; &quot;items&quot; &quot;:&quot; starting
        ///                | &quot;Operation&quot; &quot;:&quot; &quot;new&quot; &quot;=&quot; &quot;old&quot; operation
        ///                | &quot;Tes [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day00Grammar {
            get {
                return ResourceManager.GetString("Day00Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string Day00Inputs {
            get {
                return ResourceManager.GetString("Day00Inputs", resourceCulture);
            }
        }
    }
}
