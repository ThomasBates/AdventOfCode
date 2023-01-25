﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AoC.Puzzles2017.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AoC.Puzzles2017.Properties.Resources", typeof(Resources).Assembly);
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
        ///Id              = [_A-Za-z][_A-Za-z0-9]*
        ///String          = &apos;[.]*&apos;
        ///Integer         = (\+|-)?[0-9]+
        ///Real            = (\+|-)?[0-9]+\.[0-9]+
        ///
        ///#GRAMMAR
        ///
        ///program         = line moreLines
        ///
        ///moreLines       =
        ///                | line moreLines
        ///
        ///line            = &quot;line&quot;
        ///
        ///#END
        ///.
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
        
        /// <summary>
        ///   Looks up a localized string similar to 1122
        ///1111
        ///1234
        ///91212129
        ///1212
        ///1221
        ///123425
        ///123123
        ///12131415.
        /// </summary>
        internal static string Day01Inputs {
            get {
                return ResourceManager.GetString("Day01Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 5 1 9 5
        ///7 5 3
        ///2 4 6 8.
        /// </summary>
        internal static string Day02Inputs01 {
            get {
                return ResourceManager.GetString("Day02Inputs01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 5 9 2 8
        ///9 4 7 3
        ///3 8 6 5.
        /// </summary>
        internal static string Day02Inputs02 {
            get {
                return ResourceManager.GetString("Day02Inputs02", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to aa bb cc dd ee
        ///aa bb cc dd aa
        ///aa bb cc dd aaa.
        /// </summary>
        internal static string Day04Inputs01 {
            get {
                return ResourceManager.GetString("Day04Inputs01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to abcde fghij
        ///abcde xyz ecdab
        ///a ab abc abd abf abj
        ///iiii oiii ooii oooi oooo
        ///oiii ioii iioi iiio.
        /// </summary>
        internal static string Day04Inputs02 {
            get {
                return ResourceManager.GetString("Day04Inputs02", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 0
        ///3
        ///0
        ///1
        ///-3.
        /// </summary>
        internal static string Day05Inputs {
            get {
                return ResourceManager.GetString("Day05Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 0 2 7 0.
        /// </summary>
        internal static string Day06Inputs {
            get {
                return ResourceManager.GetString("Day06Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///#DEFINITIONS
        ///
        ///Id              = [_A-Za-z][_A-Za-z0-9]*
        ///String          = &apos;[.]*&apos;
        ///Integer         = (\+|-)?[0-9]+
        ///Real            = (\+|-)?[0-9]+\.[0-9]+
        ///
        ///#GRAMMAR
        ///
        ///program         = line moreLines
        ///
        ///moreLines       =
        ///                | line moreLines
        ///
        ///line            = Id s_node &quot;(&quot; Integer &quot;)&quot; c_weight above
        ///
        ///above           =
        ///                | &quot;-&gt;&quot; Id c_above above
        ///                | &quot;,&quot; Id c_above above
        ///
        ///#END
        ///.
        /// </summary>
        internal static string Day07Grammar {
            get {
                return ResourceManager.GetString("Day07Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to pbga (66)
        ///xhth (57)
        ///ebii (61)
        ///havc (66)
        ///ktlj (57)
        ///fwft (72) -&gt; ktlj, cntj, xhth
        ///qoyq (66)
        ///padx (45) -&gt; pbga, havc, qoyq
        ///tknk (41) -&gt; ugml, padx, fwft
        ///jptl (61)
        ///ugml (68) -&gt; gyxo, ebii, jptl
        ///gyxo (61)
        ///cntj (57).
        /// </summary>
        internal static string Day07Inputs {
            get {
                return ResourceManager.GetString("Day07Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to b inc 5 if a &gt; 1
        ///a inc 1 if b &lt; 5
        ///c dec -10 if a &gt;= 1
        ///c inc -20 if c == 10
        ///.
        /// </summary>
        internal static string Day08Inputs {
            get {
                return ResourceManager.GetString("Day08Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {}
        ///{{{}}}
        ///{{},{}}
        ///{{{},{},{{}}}}
        ///{&lt;a&gt;,&lt;a&gt;,&lt;a&gt;,&lt;a&gt;}
        ///{{&lt;ab&gt;},{&lt;ab&gt;},{&lt;ab&gt;},{&lt;ab&gt;}}
        ///{{&lt;!!&gt;},{&lt;!!&gt;},{&lt;!!&gt;},{&lt;!!&gt;}}
        ///{{&lt;a!&gt;},{&lt;a!&gt;},{&lt;a!&gt;},{&lt;ab&gt;}}.
        /// </summary>
        internal static string Day09Inputs {
            get {
                return ResourceManager.GetString("Day09Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 3, 4, 1, 5
        ///
        ///AoC 2017
        ///1,2,3
        ///1,2,4.
        /// </summary>
        internal static string Day10Inputs {
            get {
                return ResourceManager.GetString("Day10Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ne,ne,ne
        ///ne,ne,sw,sw
        ///ne,ne,s,s
        ///se,sw,se,sw,sw.
        /// </summary>
        internal static string Day11Inputs {
            get {
                return ResourceManager.GetString("Day11Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 0 &lt;-&gt; 2
        ///1 &lt;-&gt; 1
        ///2 &lt;-&gt; 0, 3, 4
        ///3 &lt;-&gt; 2, 4
        ///4 &lt;-&gt; 2, 3, 6
        ///5 &lt;-&gt; 6
        ///6 &lt;-&gt; 4, 5.
        /// </summary>
        internal static string Day12Inputs {
            get {
                return ResourceManager.GetString("Day12Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 0: 3
        ///1: 2
        ///4: 4
        ///6: 4.
        /// </summary>
        internal static string Day13Inputs {
            get {
                return ResourceManager.GetString("Day13Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Generator A starts with 65
        ///Generator B starts with 8921.
        /// </summary>
        internal static string Day15Inputs {
            get {
                return ResourceManager.GetString("Day15Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to s1,x3/4,pe/b.
        /// </summary>
        internal static string Day16Inputs {
            get {
                return ResourceManager.GetString("Day16Inputs", resourceCulture);
            }
        }
    }
}
