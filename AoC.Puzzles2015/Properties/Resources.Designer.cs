﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AoC.Puzzles2015.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AoC.Puzzles2015.Properties.Resources", typeof(Resources).Assembly);
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
        
        /// <summary>
        ///   Looks up a localized string similar to (())
        ///()()
        ///(((
        ///(()(()(
        ///))(((((
        ///())
        ///))(
        ///)))
        ///)())())
        ///)
        ///()()).
        /// </summary>
        internal static string Day01Inputs {
            get {
                return ResourceManager.GetString("Day01Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 2x3x4
        ///1x1x10.
        /// </summary>
        internal static string Day02Inputs {
            get {
                return ResourceManager.GetString("Day02Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &gt;
        ///^v
        ///^&gt;v&lt;
        ///^v^v^v^v^v.
        /// </summary>
        internal static string Day03Inputs {
            get {
                return ResourceManager.GetString("Day03Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to abcdef
        ///pqrstuv.
        /// </summary>
        internal static string Day04Inputs {
            get {
                return ResourceManager.GetString("Day04Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ugknbfddgicrmopn
        ///aaa
        ///jchzalrnumimnmhp
        ///haegwjzuvuyypxyu
        ///dvszwmarrgswjxmb
        ///
        ///qjhvhtzxzqqjkmpb
        ///xxyxx
        ///uurcxstgmygtbstg
        ///ieodomkazucvgmuy.
        /// </summary>
        internal static string Day05Inputs {
            get {
                return ResourceManager.GetString("Day05Inputs", resourceCulture);
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
        ///line            = &quot;turn&quot; onoff
        ///                | &quot;toggle&quot; range c_toggle
        ///
        ///onoff           = &quot;on&quot; range c_turnon
        ///                | &quot;off&quot; range c_turnoff
        ///
        ///range           = Intege [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day06Grammar {
            get {
                return ResourceManager.GetString("Day06Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to turn on 0,0 through 999,999
        ///toggle 0,0 through 999,0
        ///turn off 499,499 through 500,500.
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
        ///line            = input operation
        ///                | &quot;NOT&quot; input &quot;-&quot; &quot;&gt;&quot; Id c_not
        ///
        ///operation       = &quot;-&quot; &quot;&gt;&quot; Id c_set
        ///                | &quot;AND&quot; input &quot;-&quot; &quot;&gt;&quot; Id c_and
        ///               [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day07Grammar {
            get {
                return ResourceManager.GetString("Day07Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 123 -&gt; x
        ///456 -&gt; y
        ///x AND y -&gt; d
        ///x OR y -&gt; e
        ///x LSHIFT 2 -&gt; f
        ///y RSHIFT 2 -&gt; g
        ///NOT x -&gt; h
        ///NOT y -&gt; i.
        /// </summary>
        internal static string Day07Inputs {
            get {
                return ResourceManager.GetString("Day07Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;&quot;
        ///&quot;abc&quot;
        ///&quot;aaa\&quot;aaa&quot;
        ///&quot;\x27&quot;.
        /// </summary>
        internal static string Day08Inputs {
            get {
                return ResourceManager.GetString("Day08Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to London to Dublin = 464
        ///London to Belfast = 518
        ///Dublin to Belfast = 141.
        /// </summary>
        internal static string Day09Inputs {
            get {
                return ResourceManager.GetString("Day09Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1
        ///11
        ///21
        ///211
        ///1211
        ///111221.
        /// </summary>
        internal static string Day10Inputs {
            get {
                return ResourceManager.GetString("Day10Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to abcdefgh
        ///ghijklmn.
        /// </summary>
        internal static string Day11Inputs {
            get {
                return ResourceManager.GetString("Day11Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///#DEFINITIONS
        ///
        ///Id              = &quot;an identifier&quot; | [_A-Za-z][_A-Za-z0-9]*
        ///String          = &quot;a string&quot; | \&quot;.*\&quot;
        ///Integer         = &quot;an integer&quot; | [0-9]+
        ///
        ///#GRAMMAR
        ///
        ///program         = value moreValue
        ///
        ///value           = String t_checkForRed
        ///                | number c_number
        ///                | array
        ///                | class
        ///
        ///moreValue       = 
        ///                | &quot;,&quot; value moreValue
        ///
        ///number          = Integer
        ///                | &quot;-&quot; Integer c_negate
        ///
        ///array           = &quot;[&quot; s_beginArray moreArray s [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day12Grammar {
            get {
                return ResourceManager.GetString("Day12Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [1,2,3]
        ///{&quot;a&quot;:2,&quot;b&quot;:4}
        ///[[[3]]]
        ///{&quot;a&quot;:{&quot;b&quot;:4},&quot;c&quot;:-1}
        ///{&quot;a&quot;:[-1,1]}
        ///[-1,{&quot;a&quot;:1}]
        ///[]
        ///{} 
        ///[1,2,3]
        ///[1,{&quot;c&quot;:&quot;red&quot;,&quot;b&quot;:2},3]
        ///{&quot;d&quot;:&quot;red&quot;,&quot;e&quot;:[1,2,3,4],&quot;f&quot;:5}
        ///[1,&quot;red&quot;,5].
        /// </summary>
        internal static string Day12Inputs {
            get {
                return ResourceManager.GetString("Day12Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Alice would gain 54 happiness units by sitting next to Bob.
        ///Alice would lose 79 happiness units by sitting next to Carol.
        ///Alice would lose 2 happiness units by sitting next to David.
        ///Bob would gain 83 happiness units by sitting next to Alice.
        ///Bob would lose 7 happiness units by sitting next to Carol.
        ///Bob would lose 63 happiness units by sitting next to David.
        ///Carol would lose 62 happiness units by sitting next to Alice.
        ///Carol would gain 60 happiness units by sitting next to Bob.
        ///Carol would gain 55  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day13Inputs {
            get {
                return ResourceManager.GetString("Day13Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
        ///Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds..
        /// </summary>
        internal static string Day14Inputs {
            get {
                return ResourceManager.GetString("Day14Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
        ///Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3.
        /// </summary>
        internal static string Day15Inputs {
            get {
                return ResourceManager.GetString("Day15Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 20
        ///15
        ///10
        ///5
        ///5.
        /// </summary>
        internal static string Day17Inputs {
            get {
                return ResourceManager.GetString("Day17Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .#.#.#
        ///...##.
        ///#....#
        ///..#...
        ///#.#..#
        ///####...
        /// </summary>
        internal static string Day18Inputs {
            get {
                return ResourceManager.GetString("Day18Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to e =&gt; H
        ///e =&gt; O
        ///H =&gt; HO
        ///H =&gt; OH
        ///O =&gt; HH
        ///
        ///HOHOHO.
        /// </summary>
        internal static string Day19Inputs {
            get {
                return ResourceManager.GetString("Day19Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 200.
        /// </summary>
        internal static string Day20Inputs {
            get {
                return ResourceManager.GetString("Day20Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hit Points: 13
        ///Damage: 8
        ///
        ///Hero HP: 10
        ///Mana: 250.
        /// </summary>
        internal static string Day22Inputs13 {
            get {
                return ResourceManager.GetString("Day22Inputs13", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hit Points: 13
        ///Damage: 8
        ///
        ///Hero HP: 10
        ///Mana: 250.
        /// </summary>
        internal static string Day22Inputs14 {
            get {
                return ResourceManager.GetString("Day22Inputs14", resourceCulture);
            }
        }
    }
}
