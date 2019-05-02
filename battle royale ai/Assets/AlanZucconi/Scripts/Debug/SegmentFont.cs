using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


// Usage Example
//                    position           message string           color       fontsize
// SegmentFont.Print(new Vector2(0,0), "Message you wanna print", Color.green, 0.2f);

public static class SegmentFont
{
    private static Vector3[] points; // local space vertices for the 9 points used in the glyph segments
    private static Segment[] segments; // index buffers for each segment.
    private static Dictionary<char, short> font; // maps chars to bitfields that represent wich segments should be rendered for each glyph
    
    // you can see a map of how the points and segments align to glyphs in this image https://twitter.com/LotteMakesStuff/status/1046987391741898753
    
    private static readonly float width = 1.0f; // the default size of each character glyph
    private static readonly float height = 1.67f;
    private static float letterGap = 0.3f; // default spacing between printed glyphs
    
    struct Segment
    {
        public int a, b;

        public Segment(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
    }
    
    static SegmentFont()
    {
        var halfHeight = height / 2;
        var halfWidth = width / 2;

        
        points = new[]
        {
            new Vector3(-halfWidth, halfHeight, 0) , new Vector3(0, halfHeight, 0) , new Vector3(halfWidth, halfHeight, 0), // 0,1,2
            new Vector3(-halfWidth, 0, 0)          , new Vector3(0, 0, 0)          , new Vector3(halfWidth, 0, 0),          // 3,4,5
            new Vector3(-halfWidth, -halfHeight, 0), new Vector3(0, -halfHeight, 0), new Vector3(halfWidth, -halfHeight, 0) // 6,7,8
        };

        segments = new[]
        {
            new Segment(0,2),
            new Segment(0,3),
            new Segment(0,4),
            new Segment(1,4),
            new Segment(2,4),
            new Segment(2,5),
            new Segment(3,4), 
            new Segment(4,5),
            new Segment(3,6),
            new Segment(4,6),
            new Segment(4,7),
            new Segment(4,8),
            new Segment(5,8),
            new Segment(6,8), 
        };

        font = new Dictionary<char, short>();
//        short a = 0b_0_0__0__0__0_0_0_0_0_0_0_0_0_0; font.Add('a', a);
        
        //          13 12 11 10 9 8 7 6 5 4 3 2 1 0
        //short a = 0b_0_1__0__0__0_1_1_1_1_0_0_0_1_1; font.Add('a', a);
        //short b = 0b_1_1__0__1__0_0_1_0_1_0_1_0_0_1; font.Add('b', b);
        //short c = 0b_1_0__0__0__0_1_0_0_0_0_0_0_1_1; font.Add('c', c);
        //short d = 0b_1_1__0__1__0_0_0_0_1_0_1_0_0_1; font.Add('d', d);
        //short e = 0b_1_0__0__0__0_1_0_1_0_0_0_0_1_1; font.Add('e', e);
        short a = 4579; font.Add('a', a);
        short b = 13481; font.Add('b', b);
        short c = 8451; font.Add('c', c);
        short d = 13353; font.Add('d', d);
        short e = 8515; font.Add('e', e);
        

        //          13 12 11 10 9 8 7 6 5 4 3 2 1 0
        //short f = 0b_0_0__0__0__0_1_0_1_0_0_0_0_1_1; font.Add('f', f);
        //short g = 0b_1_1__0__0__0_1_1_0_0_0_0_0_1_1; font.Add('g', g);
        //short h = 0b_0_1__0__0__0_1_1_1_1_0_0_0_1_0; font.Add('h', h);
        //short i = 0b_1_0__0__1__0_0_0_0_0_0_1_0_0_1; font.Add('i', i);
        //short j = 0b_1_1__0__0__0_1_0_0_1_0_0_0_0_0; font.Add('j', j);
        short f = 323; font.Add('f', f);
        short g = 12675; font.Add('g', g);
        short h = 4578; font.Add('h', h);
        short i = 9225; font.Add('i', i);
        short j = 12576; font.Add('j', j);

        //          13 12 11 10 9 8 7 6 5 4 3 2 1 0
        //short k = 0b_0_0__1__0__0_1_0_1_0_1_0_0_1_0; font.Add('k', k);
        //short l = 0b_1_0__0__0__0_1_0_0_0_0_0_0_1_0; font.Add('l', l);
        //short m = 0b_0_1__0__0__0_1_0_0_1_1_0_1_1_0; font.Add('m', m);
        //short n = 0b_0_1__1__0__0_1_0_0_1_0_0_1_1_0; font.Add('n', n);
        //short o = 0b_1_1__0__0__0_1_0_0_1_0_0_0_1_1; font.Add('o', o);
        short k = 2386; font.Add('k', k);
        short l = 8450; font.Add('l', l);
        short m = 4406; font.Add('m', m);
        short n = 6438; font.Add('n', n);
        short o = 12579; font.Add('o', o);

        //          13 12 11 10 9 8 7 6 5 4 3 2 1 0
        //short p = 0b_0_0__0__0__0_1_1_1_1_0_0_0_1_1; font.Add('p', p);
        //short q = 0b_1_1__1__0__0_1_0_0_1_0_0_0_1_1; font.Add('q', q);
        //short r = 0b_0_0__1__0__0_1_1_1_1_0_0_0_1_1; font.Add('r', r);
        //short s = 0b_1_1__0__0__0_0_1_1_0_0_0_0_1_1; font.Add('s', s);
        //short t = 0b_0_0__0__1__0_0_0_0_0_0_1_0_0_1; font.Add('t', t);
        short p = 483; font.Add('p', p);
        short q = 14627; font.Add('q', q);
        short r = 2531; font.Add('r', r);
        short s = 12483; font.Add('s', s);
        short t = 1033; font.Add('t', t);

        //          13 12 11 10 9 8 7 6 5 4 3 2 1 0
        //short u = 0b_1_1__0__0__0_1_0_0_1_0_0_0_1_0; font.Add('u', u);
        //short v = 0b_0_0__0__0__1_1_0_0_0_1_0_0_1_0; font.Add('v', v);
        //short w = 0b_0_1__1__0__1_1_0_0_1_0_0_0_1_0; font.Add('w', w);
        //short x = 0b_0_0__1__0__1_0_0_0_0_1_0_1_0_0; font.Add('x', x);
        //short y = 0b_0_0__0__1__0_0_0_0_0_1_0_1_0_0; font.Add('y', y);
        //short z = 0b_1_0__0__0__1_0_0_0_0_1_0_0_0_1; font.Add('z', z);
        short u = 12578; font.Add('u', u);
        short v = 786; font.Add('v', v);
        short w = 6946; font.Add('w', w);
        short x = 2580; font.Add('x', x);
        short y = 1044; font.Add('y', y);
        short z = 8721; font.Add('z', z);

        //           13 12 11 10 9 8 7 6 5 4 3 2 1 0
        //short d0 = 0b_1_1__0__0__1_1_0_0_1_1_0_0_1_1; font.Add('0', d0);
        //short d1 = 0b_0_1__0__0__0_0_0_0_1_1_0_0_0_0; font.Add('1', d1);
        //short d2 = 0b_1_0__0__0__0_1_1_1_1_0_0_0_0_1; font.Add('2', d2);
        //short d3 = 0b_1_1__0__0__0_0_1_0_1_0_0_0_0_1; font.Add('3', d3);
        //short d4 = 0b_0_1__0__0__0_0_1_1_1_0_0_0_1_0; font.Add('4', d4);
        short d0 = 13107; font.Add('0', d0);
        short d1 = 4144; font.Add('1', d1);
        short d2 = 8673; font.Add('2', d2);
        short d3 = 12449; font.Add('3', d3);
        short d4 = 4322; font.Add('4', d4);

        //           13 12 11 10 9 8 7 6 5 4 3 2 1 0
        //short d5 = 0b_1_1__0__0__0_0_1_1_0_0_0_0_1_1; font.Add('5', d5);
        //short d6 = 0b_1_1__0__0__0_1_1_1_0_0_0_0_1_1; font.Add('6', d6);
        //short d7 = 0b_0_1__0__0__0_0_0_0_1_0_0_0_0_1; font.Add('7', d7);
        //short d8 = 0b_1_1__0__0__0_1_1_1_1_0_0_0_1_1; font.Add('8', d8);
        //short d9 = 0b_1_1__0__0__0_0_1_1_1_0_0_0_1_1; font.Add('9', d9);
        short d5 = 12483; font.Add('5', d5);
        short d6 = 12739; font.Add('6', d6);
        short d7 = 4129; font.Add('7', d7);
        short d8 = 12771; font.Add('8', d8);
        short d9 = 12515; font.Add('9', d9);

        //           13 12 11 10 9 8 7 6 5 4 3 2 1 0
        //short dash = 0b_0_0__0__0__0_0_1_0_0_0_0_0_0_0; font.Add('-', dash);
        //short less = 0b_0_0__1__0__0_0_0_0_0_1_0_0_0_0; font.Add('<', less);
       //short great = 0b_0_0__0__0__1_0_0_0_0_0_0_1_0_0; font.Add('>', great);
        short dash = 128; font.Add('-', dash);
        short less = 2064; font.Add('<', less);
        short great = 516; font.Add('>', great);
    }

    public static void Print(Vector2 pos, string message, float size = 1)
    {
        Print(pos, message, Color.red, size);
    }
    
    public static void Print(Vector2 pos, string message, Color color, float size = 1, float duration = 0f)
    {
        message = message.ToLower();
        //short missingGlyph = 0b_11111111111111;
        short missingGlyph = 16383;
        foreach (char c in message)
        {
            short glyph; 
            font.TryGetValue(c, out glyph);
            if (glyph != default(short)) // if we found the glyph in the lookup table..
            {
                PrintCharacter(glyph, pos, size, color, duration);
            }
            else if (glyph == default(short) && c != ' ') // if we didnt and the glyph is NOT a space, print the missing glyph
            {
                PrintCharacter(missingGlyph, pos, size, color, duration);
            }
            
                
            
            pos = new Vector2(pos.x + ((width+letterGap)*size), pos.y);
        }
    }

    private static void PrintCharacter(short c, Vector2 pos, float size, Color color, float duration = 0f)
    {
        short mask = 1;
        for (int i = 0; i < 14; i++) // mask out the control bit for each of our 14 segments. if its on, draw the segment
        {
            if ((c & mask) == mask)
            {
                // sctuslly draw the segment with Debug.DrawLine. 
                var edge = segments[i];
                Vector3 a = (points[edge.a] * size) + (Vector3) pos;
                Vector3 b = (points[edge.b] * size) + (Vector3) pos;
                Debug.DrawLine(a, b, color, duration);
            }
            
            // shift the mask bit up one place for next run..
            mask = (short) (mask << 1);
        }
    }
}
