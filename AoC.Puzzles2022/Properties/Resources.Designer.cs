﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AoC.Puzzles2022.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AoC.Puzzles2022.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string Day00ExampleInputs {
            get {
                return ResourceManager.GetString("Day00ExampleInputs", resourceCulture);
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
        internal static string Day00PuzzleInputs {
            get {
                return ResourceManager.GetString("Day00PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1000
        ///2000
        ///3000
        ///
        ///4000
        ///
        ///5000
        ///6000
        ///
        ///7000
        ///8000
        ///9000
        ///
        ///10000
        ///
        ///.
        /// </summary>
        internal static string Day01ExampleInputs {
            get {
                return ResourceManager.GetString("Day01ExampleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 6471
        ///1935
        ///1793
        ///3843
        ///6059
        ///6736
        ///6101
        ///3133
        ///6861
        ///1330
        ///1962
        ///5538
        ///6760
        ///
        ///5212
        ///2842
        ///3684
        ///6198
        ///6198
        ///3440
        ///2179
        ///1432
        ///5647
        ///5324
        ///6331
        ///4061
        ///1167
        ///1821
        ///
        ///7746
        ///4911
        ///3446
        ///7292
        ///4851
        ///1207
        ///5124
        ///4014
        ///1551
        ///1020
        ///4794
        ///
        ///22099
        ///26488
        ///
        ///30132
        ///22150
        ///
        ///10263
        ///14859
        ///11428
        ///
        ///9009
        ///9270
        ///2093
        ///10969
        ///5537
        ///7775
        ///8872
        ///
        ///12426
        ///3539
        ///9551
        ///6735
        ///6278
        ///
        ///5917
        ///3832
        ///2915
        ///2811
        ///1226
        ///5943
        ///4468
        ///5149
        ///3310
        ///5746
        ///4377
        ///1675
        ///2142
        ///1941
        ///5302
        ///
        ///19067
        ///18313
        ///24663
        ///
        ///6058
        ///7858
        ///3688
        ///1721
        ///1411
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day01PuzzleInputs {
            get {
                return ResourceManager.GetString("Day01PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A Y
        ///B X
        ///C Z.
        /// </summary>
        internal static string Day02ExampleInputs {
            get {
                return ResourceManager.GetString("Day02ExampleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to C X
        ///C Y
        ///C X
        ///B X
        ///B Z
        ///A Z
        ///C Y
        ///C Z
        ///B Z
        ///C X
        ///B Y
        ///C Y
        ///C Y
        ///A Y
        ///C Y
        ///C Y
        ///C Z
        ///C X
        ///B Z
        ///C Y
        ///A Y
        ///A Y
        ///C Z
        ///B Y
        ///A Y
        ///C Z
        ///C Y
        ///A Y
        ///A Y
        ///B Y
        ///C Y
        ///C Z
        ///C Y
        ///B X
        ///B Z
        ///C Y
        ///B Z
        ///A X
        ///C Z
        ///A Y
        ///B Y
        ///C Y
        ///C Y
        ///B Z
        ///B Y
        ///A Z
        ///C X
        ///C X
        ///C Y
        ///C X
        ///B Z
        ///A Y
        ///B X
        ///B Z
        ///C Z
        ///C X
        ///C X
        ///B Z
        ///A Y
        ///B Y
        ///C Y
        ///C Y
        ///A Y
        ///C X
        ///A Y
        ///B Z
        ///C Y
        ///C Y
        ///B Y
        ///C Y
        ///A Z
        ///A Z
        ///B X
        ///A Y
        ///C Y
        ///A Y
        ///C Y
        ///C Y
        ///C X
        ///C Y
        ///B Z
        ///C Y
        ///C Z
        ///C X
        ///B X
        ///C Y
        ///C Y
        ///C X
        ///C Z
        ///A Y
        ///C X
        ///B Z
        ///C X
        ///A Y
        ///B Y
        ///C Y
        ///A Y
        ///A Y
        ///A Y
        ///B Y
        ///C Y
        ///A Y
        ///A  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day02PuzzleInputs {
            get {
                return ResourceManager.GetString("Day02PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to vJrwpWtwJgWrhcsFMMfFFhFp
        ///jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
        ///PmmdzqPrVvPwwTWBwg
        ///wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
        ///ttgJtRGJQctTZtZT
        ///CrZsJsPPZsGzwwsLwLmpwMDw.
        /// </summary>
        internal static string Day03ExampleInputs {
            get {
                return ResourceManager.GetString("Day03ExampleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DsPhSBQQQhqmBDhPDsFwjwsLjlRjlttvjvvtRb
        ///rNJMNNbrHrtjHLHjvwtg
        ///fNbNzZdrZnMnMPnQShFPDmnqFm
        ///QWVCFfQffgQCVZzVVpHsHJBqtpspJFRHqq
        ///mwDbmnnGNlNcwNDDNRbnNDlJTpBJBtJGtPTLsBGqTqqsqp
        ///MlSdnScRnnmmDjSdNSdCzvggWzrgzjvfvrgVzW
        ///gsMljbrjlZlWcWMJrWwTwbmwQbmmLDQQLhwL
        ///CdgpzdgpgnfThHfFRwhfRf
        ///SptgpSpnCNpVSGNPvPGSddcMWjMrjqBsJcWqMcBWcVlZ
        ///JcJLQQFWhQJPJpWcwjHvMQvnnlMvzBHd
        ///tCtGZrmVRmVGTVTtCfRTCHHNNvdNzmdMvMlNzvwdvw
        ///CTGGRftfSGtGTGDLbFchSgSWWWcM
        ///QcMFQrvrQbvtczbVbjbMzZzRpqmDDmqqnNzCDCDC
        ///SHHfPJssGLPSdHThLhHdRmqNmNssnNmNCNnpjmsn [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day03PuzzleInputs {
            get {
                return ResourceManager.GetString("Day03PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 2-4,6-8
        ///2-3,4-5
        ///5-7,7-9
        ///2-8,3-7
        ///6-6,4-6
        ///2-6,4-8.
        /// </summary>
        internal static string Day04ExampleInputs {
            get {
                return ResourceManager.GetString("Day04ExampleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 71-87,70-88
        ///8-92,6-97
        ///7-68,8-69
        ///47-47,47-48
        ///74-94,51-93
        ///48-63,48-49
        ///26-57,57-58
        ///62-63,11-76
        ///16-66,15-47
        ///27-65,27-62
        ///19-81,19-81
        ///19-19,18-18
        ///4-5,5-15
        ///75-84,39-75
        ///96-96,20-83
        ///1-59,28-60
        ///13-14,14-88
        ///15-96,96-99
        ///4-64,1-4
        ///22-95,42-95
        ///52-53,52-78
        ///58-75,63-67
        ///25-75,63-76
        ///10-19,9-37
        ///46-81,65-65
        ///4-61,5-62
        ///66-66,67-67
        ///45-82,83-96
        ///74-96,95-96
        ///41-98,41-99
        ///22-92,21-92
        ///22-29,1-32
        ///85-86,36-84
        ///21-21,22-90
        ///13-83,15-82
        ///75-75,41-75
        ///78-78,1-78
        ///14-93,14-48
        ///58-87,58-58
        ///16-87,88-90
        ///23-64,24 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day04PuzzleInputs {
            get {
                return ResourceManager.GetString("Day04PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     [D]    
        ///[N] [C]    
        ///[Z] [M] [P]
        /// 1   2   3 
        ///
        ///move 1 from 2 to 1
        ///move 3 from 1 to 3
        ///move 2 from 2 to 1
        ///move 1 from 1 to 2.
        /// </summary>
        internal static string Day05ExampleInputs {
            get {
                return ResourceManager.GetString("Day05ExampleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to                 [M]     [V]     [L]
        ///[G]             [V] [C] [G]     [D]
        ///[J]             [Q] [W] [Z] [C] [J]
        ///[W]         [W] [G] [V] [D] [G] [C]
        ///[R]     [G] [N] [B] [D] [C] [M] [W]
        ///[F] [M] [H] [C] [S] [T] [N] [N] [N]
        ///[T] [W] [N] [R] [F] [R] [B] [J] [P]
        ///[Z] [G] [J] [J] [W] [S] [H] [S] [G]
        /// 1   2   3   4   5   6   7   8   9 
        ///
        ///move 1 from 5 to 2
        ///move 7 from 7 to 1
        ///move 1 from 1 to 7
        ///move 1 from 4 to 1
        ///move 7 from 9 to 1
        ///move 1 from 3 to 7
        ///move 4 from 5 to 4
        ///move 6 from 4 to 9
        ///move 2 from 7 to  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day05PuzzleInputs {
            get {
                return ResourceManager.GetString("Day05PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to mjqjpqmgbljsphdztnvjfqwrcgsmlb
        ///bvwbjplbgvbhsrlpgdmjqwftvncz
        ///nppdvjthqldpwncqszvftbrmjlhg
        ///nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg
        ///zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw.
        /// </summary>
        internal static string Day06ExampleInputs {
            get {
                return ResourceManager.GetString("Day06ExampleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to htsslsmstsrrhlrrllqppfnpnzzqtqtjqttslttmvmsmbbnpbbznzjjmrmsrrnjjzczcfcqchcnnhrnrzrnrzzddtrrpjrprbpbwbswsqswqswsqsqffgngtgwtwbtbbhhslsrsmrmffgcgtcgcppnbpnpbnpptcptpltthjhwwlttrrlldzldzzzlssccrfrqfrfdfldfldfdzfdzzcnznwznzqqzpqzqdzzbbslbljlqjjvzjzgzqgqqqvsvffzjfzfmzzrjrhjhrhcrhhtgtmggchggcsggvtttwmmsspmpffzpfpjjnwwnpwwdttcfcmmlblwlvvqrqddcdwcwnnfqnqdnnncggflfdftddtftqthqhrrmsrmsrsffbccjnnjjgddppjmmldllhttqvqzvzrvrsslnlplrprtprtttnvnsvsvzzdndrnnnlznzcnnzwwzjjsnjnwjwqqczqcqwqppqnqqllgblbhhbbzbjzzwjzzrqzqggdppgcg [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day06PuzzleInputs {
            get {
                return ResourceManager.GetString("Day06PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to $ cd /
        ///$ ls
        ///dir a
        ///14848514 b.txt
        ///8504156 c.dat
        ///dir d
        ///$ cd a
        ///$ ls
        ///dir e
        ///29116 f
        ///2557 g
        ///62596 h.lst
        ///$ cd e
        ///$ ls
        ///584 i
        ///$ cd ..
        ///$ cd ..
        ///$ cd d
        ///$ ls
        ///4060174 j
        ///8033020 d.log
        ///5626152 d.ext
        ///7214296 k.
        /// </summary>
        internal static string Day07ExampleInputs {
            get {
                return ResourceManager.GetString("Day07ExampleInputs", resourceCulture);
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
        ///line            = &quot;$&quot; command
        ///                | &quot;dir&quot; Id c_dir
        ///                | Integer filename c_file
        ///
        ///command         = &quot;cd&quot; directory
        ///                | &quot;ls&quot; c_ls
        ///
        ///filename        = Id extension
        ///                
        ///extension       = 
        ///                | &quot;.&quot; Id c_extension [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day07Grammar {
            get {
                return ResourceManager.GetString("Day07Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///#DEFINITIONS
        ///
        ///FileName        = &quot;a file name&quot; | [_A-Za-z][_A-Za-z0-9]*(\.)?[_A-Za-z][_A-Za-z0-9]*
        ///Number          = &quot;a number&quot; | (\+|-)?[0-9]+
        ///
        ///#GRAMMAR
        ///
        ///line            = &quot;$&quot; command
        ///                | &quot;dir&quot; FileName c_dir
        ///                | Number FileName c_file
        ///
        ///command         = &quot;cd&quot; directory
        ///                | &quot;ls&quot; c_ls
        ///
        ///directory       = &quot;/&quot; c_cdRoot
        ///                | &quot;.&quot; &quot;.&quot; c_cdParent
        ///                | FileName c_cdChild
        ///
        ///#END
        ///.
        /// </summary>
        internal static string Day07Grammar2 {
            get {
                return ResourceManager.GetString("Day07Grammar2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to $ cd /
        ///$ ls
        ///149291 cgc.vzv
        ///dir cmcrzdt
        ///dir hwdvrrp
        ///26925 hwqvsl
        ///dir lsmv
        ///dir ngfllcq
        ///dir ngnzzmpc
        ///dir pwhjps
        ///dir rgwnzttf
        ///260556 tcglclw.hsn
        ///dir trvznjhb
        ///dir wgcqrc
        ///68873 whpnhm
        ///$ cd cmcrzdt
        ///$ ls
        ///dir chqllfw
        ///95243 hjpf
        ///108868 hwqvsl
        ///115004 jpppczvz.mtp
        ///dir lnsgfnbr
        ///dir pdtjlb
        ///dir rqfzvwts
        ///dir trvznjhb
        ///$ cd chqllfw
        ///$ ls
        ///56623 cgs.hbt
        ///134804 zqb.grc
        ///$ cd ..
        ///$ cd lnsgfnbr
        ///$ ls
        ///dir jtzw
        ///dir ngfllcq
        ///dir sdm
        ///dir wlsg
        ///$ cd jtzw
        ///$ ls
        ///dir nfz
        ///$ cd nfz
        ///$ ls
        ///255427 hwqvsl
        ///9414 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day07PuzzleInputs {
            get {
                return ResourceManager.GetString("Day07PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 30373
        ///25512
        ///65332
        ///33549
        ///35390.
        /// </summary>
        internal static string Day08ExampleInputs {
            get {
                return ResourceManager.GetString("Day08ExampleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 202210010310302121322210423201220000314024242432211425434422230130411300321324302223011311211020120
        ///110110101310322320101034033124303343031143435122351113353455142421341123420341013300312303121102011
        ///022222013123323313404200430243020022232555432244334344444542122134130133413044342230233130022021112
        ///112111200212221204100121022244121453412414141145154551512445525421435232112124343223122023223002202
        ///010223032133232313122112411243113131542422453412413325334141424132125341004123403342031113000321021
        ///1202033 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day08PuzzleInputs {
            get {
                return ResourceManager.GetString("Day08PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to R 4
        ///U 4
        ///L 3
        ///D 1
        ///R 4
        ///D 1
        ///L 5
        ///R 2.
        /// </summary>
        internal static string Day09ExampleInputs {
            get {
                return ResourceManager.GetString("Day09ExampleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to R 5
        ///U 8
        ///L 8
        ///D 3
        ///R 17
        ///D 10
        ///L 25
        ///U 20.
        /// </summary>
        internal static string Day09ExampleInputs2 {
            get {
                return ResourceManager.GetString("Day09ExampleInputs2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to L 1
        ///D 2
        ///R 2
        ///L 1
        ///D 1
        ///L 1
        ///U 1
        ///R 1
        ///L 2
        ///R 2
        ///L 2
        ///D 1
        ///R 2
        ///D 1
        ///U 2
        ///R 2
        ///D 1
        ///R 1
        ///L 2
        ///R 1
        ///D 1
        ///U 2
        ///R 2
        ///D 1
        ///R 2
        ///L 1
        ///D 1
        ///U 1
        ///R 1
        ///D 2
        ///L 1
        ///D 1
        ///L 1
        ///U 1
        ///L 2
        ///U 1
        ///L 1
        ///U 1
        ///L 1
        ///D 2
        ///R 2
        ///U 1
        ///D 2
        ///R 1
        ///U 1
        ///D 1
        ///R 1
        ///U 2
        ///L 2
        ///D 2
        ///R 1
        ///U 2
        ///L 2
        ///U 1
        ///D 1
        ///L 1
        ///R 2
        ///L 2
        ///R 1
        ///D 2
        ///L 1
        ///D 2
        ///L 1
        ///R 2
        ///U 2
        ///D 2
        ///U 1
        ///R 2
        ///D 2
        ///L 2
        ///U 1
        ///D 2
        ///R 1
        ///L 2
        ///R 1
        ///L 2
        ///U 2
        ///D 2
        ///U 2
        ///D 2
        ///R 1
        ///U 2
        ///L 2
        ///D 1
        ///U 2
        ///L 1
        ///D 1
        ///R 2
        ///U 1
        ///L 1
        ///D 1
        ///U 2
        ///D 2
        ///R 2
        ///U 1
        ///L 2
        ///D 2
        ///L 1
        ///D 2
        ///L 1
        ///U 1
        ///R 2
        ///L  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day09PuzzleInputs {
            get {
                return ResourceManager.GetString("Day09PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to addx 15
        ///addx -11
        ///addx 6
        ///addx -3
        ///addx 5
        ///addx -1
        ///addx -8
        ///addx 13
        ///addx 4
        ///noop
        ///addx -1
        ///addx 5
        ///addx -1
        ///addx 5
        ///addx -1
        ///addx 5
        ///addx -1
        ///addx 5
        ///addx -1
        ///addx -35
        ///addx 1
        ///addx 24
        ///addx -19
        ///addx 1
        ///addx 16
        ///addx -11
        ///noop
        ///noop
        ///addx 21
        ///addx -15
        ///noop
        ///noop
        ///addx -3
        ///addx 9
        ///addx 1
        ///addx -3
        ///addx 8
        ///addx 1
        ///addx 5
        ///noop
        ///noop
        ///noop
        ///noop
        ///noop
        ///addx -36
        ///noop
        ///addx 1
        ///addx 7
        ///noop
        ///noop
        ///noop
        ///addx 2
        ///addx 6
        ///noop
        ///noop
        ///noop
        ///noop
        ///noop
        ///addx 1
        ///noop
        ///noop
        ///addx 7
        ///addx 1
        ///noop
        ///addx -13
        ///addx 13 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day10ExampleInputs {
            get {
                return ResourceManager.GetString("Day10ExampleInputs", resourceCulture);
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
        ///line            = &quot;noop&quot; c_noop
        ///                | &quot;addx&quot; arg c_addx
        ///
        ///arg             = &quot;-&quot; Integer c_negateArg
        ///                | Integer
        ///#END
        ///.
        /// </summary>
        internal static string Day10Grammar {
            get {
                return ResourceManager.GetString("Day10Grammar", resourceCulture);
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
        ///line            = &quot;noop&quot; c_noop
        ///                | &quot;addx&quot; arg t_integer c_addx
        ///
        ///arg             = &quot;-&quot; Integer c_negate
        ///                | Integer
        ///
        ///#END
        ///.
        /// </summary>
        internal static string Day10Grammar2 {
            get {
                return ResourceManager.GetString("Day10Grammar2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to addx 1
        ///noop
        ///addx 5
        ///addx -1
        ///addx 5
        ///addx 1
        ///noop
        ///noop
        ///addx 2
        ///addx 5
        ///addx 2
        ///addx 1
        ///noop
        ///addx -21
        ///addx 26
        ///addx -6
        ///addx 8
        ///noop
        ///noop
        ///addx 7
        ///noop
        ///noop
        ///noop
        ///addx -37
        ///addx 13
        ///addx -6
        ///addx -2
        ///addx 5
        ///addx 25
        ///addx 2
        ///addx -24
        ///addx 2
        ///addx 5
        ///addx 5
        ///noop
        ///noop
        ///addx -2
        ///addx 2
        ///addx 5
        ///addx 2
        ///addx 7
        ///addx -2
        ///noop
        ///addx -8
        ///addx 9
        ///addx -36
        ///noop
        ///noop
        ///addx 5
        ///addx 6
        ///noop
        ///addx 25
        ///addx -24
        ///addx 3
        ///addx -2
        ///noop
        ///addx 3
        ///addx 6
        ///noop
        ///addx 9
        ///addx -8
        ///addx 5
        ///addx 2
        ///addx -7
        ///noop
        ///addx [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day10PuzzleInputs {
            get {
                return ResourceManager.GetString("Day10PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Monkey 0:
        ///  Starting items: 79, 98
        ///  Operation: new = old * 19
        ///  Test: divisible by 23
        ///    If true: throw to monkey 2
        ///    If false: throw to monkey 3
        ///
        ///Monkey 1:
        ///  Starting items: 54, 65, 75, 74
        ///  Operation: new = old + 6
        ///  Test: divisible by 19
        ///    If true: throw to monkey 2
        ///    If false: throw to monkey 0
        ///
        ///Monkey 2:
        ///  Starting items: 79, 60, 97
        ///  Operation: new = old * old
        ///  Test: divisible by 13
        ///    If true: throw to monkey 1
        ///    If false: throw to monkey 3
        ///
        ///Monkey 3:
        ///  Starting item [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day11ExampleInputs {
            get {
                return ResourceManager.GetString("Day11ExampleInputs", resourceCulture);
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
        internal static string Day11Grammar {
            get {
                return ResourceManager.GetString("Day11Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Monkey 0:
        ///  Starting items: 65, 58, 93, 57, 66
        ///  Operation: new = old * 7
        ///  Test: divisible by 19
        ///    If true: throw to monkey 6
        ///    If false: throw to monkey 4
        ///
        ///Monkey 1:
        ///  Starting items: 76, 97, 58, 72, 57, 92, 82
        ///  Operation: new = old + 4
        ///  Test: divisible by 3
        ///    If true: throw to monkey 7
        ///    If false: throw to monkey 5
        ///
        ///Monkey 2:
        ///  Starting items: 90, 89, 96
        ///  Operation: new = old * 5
        ///  Test: divisible by 13
        ///    If true: throw to monkey 5
        ///    If false: throw to monkey 1
        ///
        ///Monkey [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day11PuzzleInputs {
            get {
                return ResourceManager.GetString("Day11PuzzleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sabqponm
        ///abcryxxl
        ///accszExk
        ///acctuvwj
        ///abdefghi.
        /// </summary>
        internal static string Day12ExampleInputs {
            get {
                return ResourceManager.GetString("Day12ExampleInputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to abccccaaaaaaaaaaaaaccaaaaaaaacccccccccaaaaaaaaccccccccaaacaaacccccccaaaaaaccccccccccccccccccccccaaaacccccccccccacccccccccccccccccccccccccccccccccccccccccccccccaaaa
        ///abccccaaaaacaaaaaaccccaaaaaaccccccccccaaaaaaacccccccccaaaaaaacccccaaaaaaaaaacccccccccccccccccccaaaaaacccccccccaaaaaaaaccccccccccccccccccccccccccccccccccccccccaaaaa
        ///abcccaaaaaccaaaaaaccccaaaaaaccccccaacccaaaaaacccccccccaaaaaacccaaaaaaaaaaaaaaacaaccacccccccccccaaaaaaccccccccccaaaaaacccccccccccccccccccccccccccccccccccccccccaaaaa
        ///abccccccaaccaaaaa [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Day12PuzzleInputs {
            get {
                return ResourceManager.GetString("Day12PuzzleInputs", resourceCulture);
            }
        }
    }
}
