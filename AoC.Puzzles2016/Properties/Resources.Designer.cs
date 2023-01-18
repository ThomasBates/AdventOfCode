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
        
        /// <summary>
        ///   Looks up a localized string similar to R8, R4, R4, R8.
        /// </summary>
        internal static string Day01Inputs {
            get {
                return ResourceManager.GetString("Day01Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ULL
        ///RRDDD
        ///LURDL
        ///UUUUD.
        /// </summary>
        internal static string Day02Inputs {
            get {
                return ResourceManager.GetString("Day02Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to aaaaa-bbb-z-y-x-123[abxyz]
        ///a-b-c-d-e-f-g-h-987[abcde]
        ///not-a-real-room-404[oarel]
        ///totally-real-room-200[decoy].
        /// </summary>
        internal static string Day04Inputs {
            get {
                return ResourceManager.GetString("Day04Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to abc.
        /// </summary>
        internal static string Day05Inputs {
            get {
                return ResourceManager.GetString("Day05Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to eedadn
        ///drvtee
        ///eandsr
        ///raavrd
        ///atevrs
        ///tsrnev
        ///sdttsa
        ///rasrtv
        ///nssdts
        ///ntnada
        ///svetve
        ///tesnvt
        ///vntsnd
        ///vrdear
        ///dvrsen
        ///enarar.
        /// </summary>
        internal static string Day06Inputs {
            get {
                return ResourceManager.GetString("Day06Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to abba[mnop]qrst
        ///abcd[bddb]xyyx
        ///aaaa[qwer]tyui
        ///ioxxoj[asdfgh]zxcvbn.
        /// </summary>
        internal static string Day07Inputs01 {
            get {
                return ResourceManager.GetString("Day07Inputs01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to aba[bab]xyz
        ///xyx[xyx]xyx
        ///aaa[kek]eke
        ///zazbz[bzb]cdb.
        /// </summary>
        internal static string Day07Inputs02 {
            get {
                return ResourceManager.GetString("Day07Inputs02", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///#DEFINITIONS
        ///
        ///Integer         = [0-9]+
        ///
        ///#GRAMMAR
        ///
        ///program         = line moreLines
        ///
        ///moreLines       =
        ///                | line moreLines
        ///
        ///line            = &quot;rect&quot; Integer &quot;x&quot; Integer c_rect
        ///                | &quot;rotate&quot; doRotate
        ///
        ///doRotate        = &quot;row&quot; &quot;y&quot; &quot;=&quot; Integer &quot;by&quot; Integer c_rotateRow
        ///                | &quot;column&quot; &quot;x&quot; &quot;=&quot; Integer &quot;by&quot; Integer c_rotateCol
        ///
        ///#END
        ///.
        /// </summary>
        internal static string Day08Grammar {
            get {
                return ResourceManager.GetString("Day08Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to rect 3x2
        ///rotate column x=1 by 1
        ///rotate row y=0 by 4
        ///rotate column x=1 by 1.
        /// </summary>
        internal static string Day08Inputs {
            get {
                return ResourceManager.GetString("Day08Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ADVENT
        ///A(1x5)BC
        ///(3x3)XYZ
        ///A(2x2)BCD(2x2)EFG
        ///(6x1)(1x3)A
        ///X(8x2)(3x3)ABCY.
        /// </summary>
        internal static string Day09Inputs01 {
            get {
                return ResourceManager.GetString("Day09Inputs01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (3x3)XYZ
        ///X(8x2)(3x3)ABCY
        ///(27x12)(20x12)(13x14)(7x10)(1x12)A
        ///(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN.
        /// </summary>
        internal static string Day09Inputs02 {
            get {
                return ResourceManager.GetString("Day09Inputs02", resourceCulture);
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
        ///line            = &quot;value&quot; Integer &quot;goes&quot; &quot;to&quot; &quot;bot&quot; Integer c_value
        ///                | &quot;bot&quot; Integer &quot;gives&quot; &quot;low&quot; &quot;to&quot; target &quot;and&quot; &quot;high&quot; &quot;to&quot; target c_bot
        ///
        ///target          = &quot;bot [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day10Grammar {
            get {
                return ResourceManager.GetString("Day10Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to value 5 goes to bot 2
        ///bot 2 gives low to bot 1 and high to bot 0
        ///value 3 goes to bot 1
        ///bot 1 gives low to output 1 and high to bot 0
        ///bot 0 gives low to output 2 and high to output 0
        ///value 2 goes to bot 2.
        /// </summary>
        internal static string Day10Inputs {
            get {
                return ResourceManager.GetString("Day10Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///#DEFINITIONS
        ///
        ///Id              = [_A-Za-z][_A-Za-z0-9]*
        ///
        ///#GRAMMAR
        ///
        ///program         = line moreLines
        ///
        ///moreLines       =
        ///                | line moreLines
        ///
        ///line            = &quot;The&quot; numberWord &quot;floor&quot; s_floor &quot;contains&quot; floorContents &quot;.&quot;
        ///
        ///numberWord      = &quot;first&quot; t_first
        ///                | &quot;second&quot; t_second
        ///                | &quot;third&quot; t_third
        ///                | &quot;fourth&quot; t_fourth
        ///
        ///floorContents   = article contents moreContents
        ///                | &quot;nothing&quot; &quot;relevant&quot;  c_nothing
        ///
        ///article          [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day11Grammar {
            get {
                return ResourceManager.GetString("Day11Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.
        ///The second floor contains a hydrogen generator.
        ///The third floor contains a lithium generator.
        ///The fourth floor contains nothing relevant..
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
        ///Integer         = &quot;an integer&quot; | (\+|-)?[0-9]+
        ///
        ///#GRAMMAR
        ///
        ///program         = line moreLines
        ///
        ///moreLines       =
        ///                | line moreLines
        ///
        ///line            = &quot;cpy&quot; value register c_cpy
        ///                | &quot;inc&quot; register c_inc
        ///                | &quot;dec&quot; register c_dec
        ///                | &quot;jnz&quot; value value c_jnz
        ///
        ///value           = Integer c_numberValue
        ///                | Id c_registerValue
        ///
        ///register        = Id c_regist [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day12Grammar {
            get {
                return ResourceManager.GetString("Day12Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to cpy 41 a
        ///inc a
        ///inc a
        ///dec a
        ///jnz a 2
        ///dec a.
        /// </summary>
        internal static string Day12Inputs {
            get {
                return ResourceManager.GetString("Day12Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 10.
        /// </summary>
        internal static string Day13Inputs {
            get {
                return ResourceManager.GetString("Day13Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to abc.
        /// </summary>
        internal static string Day14Inputs {
            get {
                return ResourceManager.GetString("Day14Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Disc #1 has 5 positions; at time=0, it is at position 4.
        ///Disc #2 has 2 positions; at time=0, it is at position 1..
        /// </summary>
        internal static string Day15Inputs {
            get {
                return ResourceManager.GetString("Day15Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 10000
        ///20.
        /// </summary>
        internal static string Day16Inputs {
            get {
                return ResourceManager.GetString("Day16Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 5-8
        ///0-2
        ///4-7.
        /// </summary>
        internal static string Day20Inputs {
            get {
                return ResourceManager.GetString("Day20Inputs", resourceCulture);
            }
        }
    }
}
