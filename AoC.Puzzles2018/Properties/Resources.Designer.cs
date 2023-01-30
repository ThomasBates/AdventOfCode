﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AoC.Puzzles2018.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AoC.Puzzles2018.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to +1
        ///-2
        ///+3
        ///+1.
        /// </summary>
        internal static string Day01Inputs {
            get {
                return ResourceManager.GetString("Day01Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to abcde
        ///fghij
        ///klmno
        ///pqrst
        ///fguij
        ///axcye
        ///wvxyz.
        /// </summary>
        internal static string Day02Inputs {
            get {
                return ResourceManager.GetString("Day02Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///#DEFINITIONS
        ///
        ///Integer         = &quot;an integer&quot; | [0-9]+
        ///
        ///#GRAMMAR
        ///
        ///line            = claimId location size
        ///
        ///claimId         = &quot;#&quot; Integer c_claimId
        ///
        ///location        = &quot;@&quot; Integer c_left &quot;,&quot; Integer c_top
        ///
        ///size            = &quot;:&quot; Integer c_width &quot;x&quot; Integer c_height
        ///
        ///#END
        ///.
        /// </summary>
        internal static string Day03Grammar {
            get {
                return ResourceManager.GetString("Day03Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #1 @ 1,3: 4x4
        ///#2 @ 3,1: 4x4
        ///#3 @ 5,5: 2x2.
        /// </summary>
        internal static string Day03Inputs {
            get {
                return ResourceManager.GetString("Day03Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///#DEFINITIONS
        ///
        ///Id              = &quot;an identifier&quot; | [_A-Za-z][_A-Za-z0-9]*
        ///String          = &quot;a string&quot; | &apos;[.]*&apos;
        ///Integer         = &quot;an integer&quot; | [0-9]+
        ///Real            = &quot;a real number&quot; | [0-9]+\.[0-9]+
        ///
        ///#GRAMMAR
        ///
        ///line            = timestamp event
        ///
        ///timestamp       = &quot;[&quot; Integer c_year &quot;-&quot; Integer c_month &quot;-&quot; Integer c_day Integer c_hour &quot;:&quot; Integer c_minute &quot;]&quot; t_timestamp
        ///
        ///event           = &quot;Guard&quot; &quot;#&quot; Integer c_guardId &quot;begins&quot; &quot;shift&quot; t_beginsShift
        ///                | &quot;falls&quot; &quot;asleep&quot; t_fall [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day04Grammar {
            get {
                return ResourceManager.GetString("Day04Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [1518-11-01 00:00] Guard #10 begins shift
        ///[1518-11-01 00:05] falls asleep
        ///[1518-11-01 00:25] wakes up
        ///[1518-11-01 00:30] falls asleep
        ///[1518-11-01 00:55] wakes up
        ///[1518-11-01 23:58] Guard #99 begins shift
        ///[1518-11-02 00:40] falls asleep
        ///[1518-11-02 00:50] wakes up
        ///[1518-11-03 00:05] Guard #10 begins shift
        ///[1518-11-03 00:24] falls asleep
        ///[1518-11-03 00:29] wakes up
        ///[1518-11-04 00:02] Guard #99 begins shift
        ///[1518-11-04 00:36] falls asleep
        ///[1518-11-04 00:46] wakes up
        ///[1518-11-05 00:03] Guard #99 b [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day04Inputs {
            get {
                return ResourceManager.GetString("Day04Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to dabAcCaCBAcCcaDA.
        /// </summary>
        internal static string Day05Inputs {
            get {
                return ResourceManager.GetString("Day05Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1, 1
        ///1, 6
        ///8, 3
        ///3, 4
        ///5, 5
        ///8, 9.
        /// </summary>
        internal static string Day06Inputs {
            get {
                return ResourceManager.GetString("Day06Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Step C must be finished before step A can begin.
        ///Step C must be finished before step F can begin.
        ///Step A must be finished before step B can begin.
        ///Step A must be finished before step D can begin.
        ///Step B must be finished before step E can begin.
        ///Step D must be finished before step E can begin.
        ///Step F must be finished before step E can begin..
        /// </summary>
        internal static string Day07Inputs {
            get {
                return ResourceManager.GetString("Day07Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2.
        /// </summary>
        internal static string Day08Inputs {
            get {
                return ResourceManager.GetString("Day08Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 9 players; last marble is worth 25 points: high score is 32
        ///10 players; last marble is worth 1618 points: high score is 8317
        ///13 players; last marble is worth 7999 points: high score is 146373
        ///17 players; last marble is worth 1104 points: high score is 2764
        ///21 players; last marble is worth 6111 points: high score is 54718
        ///30 players; last marble is worth 5807 points: high score is 37305.
        /// </summary>
        internal static string Day09Inputs {
            get {
                return ResourceManager.GetString("Day09Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///#DEFINITIONS
        ///
        ///Id              = &quot;an identifier&quot; | [_A-Za-z][_A-Za-z0-9]*
        ///String          = &quot;a string&quot; | &apos;[.]*&apos;
        ///Integer         = &quot;an integer&quot; | (\+|-)?[0-9]+
        ///Real            = &quot;a real number&quot; | [0-9]+\.[0-9]+
        ///
        ///#GRAMMAR
        ///
        ///line            = position velocity
        ///
        ///position        = &quot;position&quot; d_test1 &quot;=&quot; d_test2 vector c_position
        ///
        ///velocity        = &quot;velocity&quot; &quot;=&quot; vector c_velocity
        ///
        ///vector          = d_test3 &quot;&lt;&quot; d_test4 Integer c_x &quot;,&quot; Integer c_y &quot;&gt;&quot;
        ///
        ///vectors         = &quot;&lt;&quot; sign Integer c_x &quot;,&quot; si [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day10Grammar {
            get {
                return ResourceManager.GetString("Day10Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to position=&lt; 9,  1&gt; velocity=&lt; 0,  2&gt;
        ///position=&lt; 7,  0&gt; velocity=&lt;-1,  0&gt;
        ///position=&lt; 3, -2&gt; velocity=&lt;-1,  1&gt;
        ///position=&lt; 6, 10&gt; velocity=&lt;-2, -1&gt;
        ///position=&lt; 2, -4&gt; velocity=&lt; 2,  2&gt;
        ///position=&lt;-6, 10&gt; velocity=&lt; 2, -2&gt;
        ///position=&lt; 1,  8&gt; velocity=&lt; 1, -1&gt;
        ///position=&lt; 1,  7&gt; velocity=&lt; 1,  0&gt;
        ///position=&lt;-3, 11&gt; velocity=&lt; 1, -2&gt;
        ///position=&lt; 7,  6&gt; velocity=&lt;-1, -1&gt;
        ///position=&lt;-2,  3&gt; velocity=&lt; 1,  0&gt;
        ///position=&lt;-4,  3&gt; velocity=&lt; 2,  0&gt;
        ///position=&lt;10, -3&gt; velocity=&lt;-1,  1&gt;
        ///position=&lt; 5, 11&gt; velocity=&lt; 1, [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day10Inputs {
            get {
                return ResourceManager.GetString("Day10Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 18
        ///42.
        /// </summary>
        internal static string Day11Inputs {
            get {
                return ResourceManager.GetString("Day11Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to initial state: #..#.#..##......###...###
        ///
        ///..... =&gt; .
        ///....# =&gt; .
        ///...#. =&gt; .
        ///...## =&gt; #
        ///..#.. =&gt; #
        ///..#.# =&gt; .
        ///..##. =&gt; .
        ///..### =&gt; .
        ///.#... =&gt; #
        ///.#..# =&gt; .
        ///.#.#. =&gt; #
        ///.#.## =&gt; #
        ///.##.. =&gt; #
        ///.##.# =&gt; .
        ///.###. =&gt; .
        ///.#### =&gt; #
        ///#.... =&gt; .
        ///#...# =&gt; .
        ///#..#. =&gt; .
        ///#..## =&gt; .
        ///#.#.. =&gt; .
        ///#.#.# =&gt; #
        ///#.##. =&gt; .
        ///#.### =&gt; #
        ///##... =&gt; .
        ///##..# =&gt; .
        ///##.#. =&gt; #
        ///##.## =&gt; #
        ///###.. =&gt; #
        ///###.# =&gt; #
        ///####. =&gt; #
        ///##### =&gt; .
        ///.
        /// </summary>
        internal static string Day12Inputs {
            get {
                return ResourceManager.GetString("Day12Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /-&gt;-\        
        ///|   |  /----\
        ///| /-+--+-\  |
        ///| | |  | v  |
        ///\-+-/  \-+--/
        ///  \------/  .
        /// </summary>
        internal static string Day13Inputs {
            get {
                return ResourceManager.GetString("Day13Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /&gt;-&lt;\  
        ///|   |  
        ///| /&lt;+-\
        ///| | | v
        ///\&gt;+&lt;/ |
        ///  |   ^
        ///  \&lt;-&gt;/.
        /// </summary>
        internal static string Day13Inputs2 {
            get {
                return ResourceManager.GetString("Day13Inputs2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 9
        ///5
        ///18
        ///2018.
        /// </summary>
        internal static string Day14Inputs {
            get {
                return ResourceManager.GetString("Day14Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 51589
        ///01245
        ///92510
        ///59414.
        /// </summary>
        internal static string Day14Inputs2 {
            get {
                return ResourceManager.GetString("Day14Inputs2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #######
        ///#.G...#
        ///#...EG#
        ///#.#.#G#
        ///#..G#E#
        ///#.....#
        ///#######.
        /// </summary>
        internal static string Day15Inputs01 {
            get {
                return ResourceManager.GetString("Day15Inputs01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #######
        ///#G..#E#
        ///#E#E.E#
        ///#G.##.#
        ///#...#E#
        ///#...E.#
        ///#######
        ///.
        /// </summary>
        internal static string Day15Inputs02 {
            get {
                return ResourceManager.GetString("Day15Inputs02", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #######
        ///#E..EG#
        ///#.#G.E#
        ///#E.##E#
        ///#G..#.#
        ///#..E#.#
        ///#######.
        /// </summary>
        internal static string Day15Inputs03 {
            get {
                return ResourceManager.GetString("Day15Inputs03", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #######
        ///#E.G#.#
        ///#.#G..#
        ///#G.#.G#
        ///#G..#.#
        ///#...E.#
        ///#######.
        /// </summary>
        internal static string Day15Inputs04 {
            get {
                return ResourceManager.GetString("Day15Inputs04", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #######
        ///#.E...#
        ///#.#..G#
        ///#.###.#
        ///#E#G#G#
        ///#...#G#
        ///#######.
        /// </summary>
        internal static string Day15Inputs05 {
            get {
                return ResourceManager.GetString("Day15Inputs05", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #########
        ///#G......#
        ///#.E.#...#
        ///#..##..G#
        ///#...##..#
        ///#...#...#
        ///#.G...G.#
        ///#.....G.#
        ///#########.
        /// </summary>
        internal static string Day15Inputs06 {
            get {
                return ResourceManager.GetString("Day15Inputs06", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ################################
        ///#######.G...####################
        ///#########...####################
        ///#########.G.####################
        ///#########.######################
        ///#########.######################
        ///#########G######################
        ///#########.#...##################
        ///#########.....#..###############
        ///########...G....###.....########
        ///#######............G....########
        ///#######G....G.....G....#########
        ///######..G.....#####..G...#######
        ///######...G...#######......######
        ///#####.......#########....G..E###
        ///## [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day15Inputs07 {
            get {
                return ResourceManager.GetString("Day15Inputs07", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ################################
        ///#####################...########
        ///###################....G########
        ///###################....#########
        ///#######.##########......########
        ///#######G#########........#######
        ///#######G#######.G.........######
        ///#######.######..G.........######
        ///#######.......##.G...G.G..######
        ///########..##..#....G......G#####
        ///############...#.....G.....#####
        ///#...#######..........G.#...#####
        ///#...#######...#####G......######
        ///##...######..#######G.....#.##.#
        ///###.G.#####.#########G.........#
        ///## [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day15Inputs08 {
            get {
                return ResourceManager.GetString("Day15Inputs08", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ################################
        ///#############################..#
        ///#############################..#
        ///#############################..#
        ///###########################...##
        ///##########################..####
        ///###########..#G.#########G..####
        ///#########G........#######....###
        ///#########...G.......G##........#
        ///#######G.................E...###
        ///#######.####...####..G..#...####
        ///######...#.........G..###....###
        ///#####....#....#####...####...###
        ///####.........#######.....#....##
        ///#.G..G.G..#G#########...G.....##
        ///#. [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day15Inputs09 {
            get {
                return ResourceManager.GetString("Day15Inputs09", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to x=495, y=2..7
        ///y=7, x=495..501
        ///x=501, y=3..7
        ///x=498, y=2..4
        ///x=506, y=1..2
        ///x=498, y=10..13
        ///x=504, y=10..13
        ///y=13, x=498..504.
        /// </summary>
        internal static string Day17Inputs01 {
            get {
                return ResourceManager.GetString("Day17Inputs01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .#.#...|#.
        ///.....#|##|
        ///.|..|...#.
        ///..|#.....#
        ///#.#|||#|#|
        ///...#.||...
        ///.|....|...
        ///||...#|.#|
        ///|.||||..|.
        ///...#.|..|..
        /// </summary>
        internal static string Day18Inputs01 {
            get {
                return ResourceManager.GetString("Day18Inputs01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ..|..|.|.|.||..#.#|...|..#.|.........|.......|..#.
        ///#.|.........|||....#....|....##||.....|.|.........
        ///..||......#.#||#.#.......#..#.#.###...|.#..#...#..
        ///|....#....|.##.##.....##...##|..|....|..|#||...###
        ///#|...|.#|..|......#.##....#|....|...|#......|.#|.|
        ///..|....##.##.#..||##...#..##|......|...|#.||.#.#..
        ///.#...#||...........#|.....|##....#.#...|#.|###..|.
        ///||....#.#.|...||...###|.|#.....#.|#.|#...#.#.|...#
        ///...#.....||.......#....#|###|####..|#|.###..||.#.#
        ///|#|...||..##.||.||..#.#.|..#...#..|........# [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day18Inputs02 {
            get {
                return ResourceManager.GetString("Day18Inputs02", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #ip 0
        ///seti 5 0 1
        ///seti 6 0 2
        ///addi 0 1 0
        ///addr 1 2 3
        ///setr 1 0 0
        ///seti 8 0 4
        ///seti 9 0 5.
        /// </summary>
        internal static string Day19Inputs01 {
            get {
                return ResourceManager.GetString("Day19Inputs01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^WNE$.
        /// </summary>
        internal static string Day20Inputs01 {
            get {
                return ResourceManager.GetString("Day20Inputs01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^ENWWW(NEEE|SSE(EE|N))$.
        /// </summary>
        internal static string Day20Inputs02 {
            get {
                return ResourceManager.GetString("Day20Inputs02", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$.
        /// </summary>
        internal static string Day20Inputs03 {
            get {
                return ResourceManager.GetString("Day20Inputs03", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$.
        /// </summary>
        internal static string Day20Inputs04 {
            get {
                return ResourceManager.GetString("Day20Inputs04", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$.
        /// </summary>
        internal static string Day20Inputs05 {
            get {
                return ResourceManager.GetString("Day20Inputs05", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to depth: 510
        ///target: 10,10.
        /// </summary>
        internal static string Day22Inputs {
            get {
                return ResourceManager.GetString("Day22Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to pos=&lt;0,0,0&gt;, r=4
        ///pos=&lt;1,0,0&gt;, r=1
        ///pos=&lt;4,0,0&gt;, r=3
        ///pos=&lt;0,2,0&gt;, r=1
        ///pos=&lt;0,5,0&gt;, r=3
        ///pos=&lt;0,0,3&gt;, r=1
        ///pos=&lt;1,1,1&gt;, r=1
        ///pos=&lt;1,1,2&gt;, r=1
        ///pos=&lt;1,3,1&gt;, r=1.
        /// </summary>
        internal static string Day23Inputs01 {
            get {
                return ResourceManager.GetString("Day23Inputs01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to pos=&lt;10,12,12&gt;, r=2
        ///pos=&lt;12,14,12&gt;, r=2
        ///pos=&lt;16,12,12&gt;, r=4
        ///pos=&lt;14,14,14&gt;, r=6
        ///pos=&lt;50,50,50&gt;, r=200
        ///pos=&lt;10,10,10&gt;, r=5.
        /// </summary>
        internal static string Day23Inputs02 {
            get {
                return ResourceManager.GetString("Day23Inputs02", resourceCulture);
            }
        }
    }
}
