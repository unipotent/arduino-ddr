using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class OsuManiaSongParser // this is a program that parses a beatmap file to copy and paste into the arduino
{
    public static void Main()
    {
        string filePath = Console.ReadLine().Replace("\"", ""); // replace quotations from when you press 'copy file as path'
        string[] text = File.ReadAllLines(filePath);

        // x, y, time, type, hitsound, [object-specific parameters], [hit-sound extras]
        // eg 320,192,1066,5,0,0:0:0:0:m_049.ogg
        // hold note looks like ths: 448,192,1600,128,0,3600:0:0:0:0:m_058.ogg
        int startIndex = Array.FindIndex(text, x => x == "[HitObjects]") + 1;
        Parser parser = new();

        for (int i = startIndex; i < text.Length; i++)
        {
            string[] hitObject = text[i].Split(',');

            // column index = floor(x * (laneCount / 512));
            int x = (int)Math.Floor(int.Parse(hitObject[0]) * (4.0 / 512.0));
            // y irrelevant
            int time = int.Parse(hitObject[2]);

            string[] extras = hitObject[hitObject.Length - 1].Split(':');
            string hitsoundFile = extras[extras.Length - 1];

            int holdTime = int.Parse(extras[0]);
            if (holdTime != 0) // actual hold time exists, its stored as when the hold note ends meanwhile ours is the hold time relative to the timestamp time.
            {
                holdTime = holdTime - time;
            }

            int frequency = ParseNoteToFrequency(Path.GetFileNameWithoutExtension(hitsoundFile));

            parser.Append(x, time, frequency, holdTime);
        }
        Console.WriteLine(parser.Finish()); // write to copy paste
    }

    public static int ParseNoteToFrequency(string noteStr)
    {
        // 1. Clean up and normalize string formatting
        noteStr = noteStr.ToUpper().Replace(" ", "");

        // 2. Fix swapped accidentals (e.g., convert "A4#" to "A#4")
        if (noteStr.EndsWith("#") || noteStr.EndsWith("B"))
        {
            char accidental = noteStr[noteStr.Length - 1];
            char octaveChar = noteStr[noteStr.Length - 2];
            noteStr = noteStr.Substring(0, noteStr.Length - 2) + accidental + octaveChar;
        }

        // 3. Find where the octave digit begins to split the string
        int octaveIndex = -1;
        for (int i = 0; i < noteStr.Length; i++)
        {
            // Account for negative octaves (e.g., C-1) safely
            if (char.IsDigit(noteStr[i]) || (noteStr[i] == '-' && i + 1 < noteStr.Length && char.IsDigit(noteStr[i + 1])))
            {
                octaveIndex = i;
                break;
            }
        }

        // 4. Extract note name and octave number
        string noteName = octaveIndex != -1 ? noteStr.Substring(0, octaveIndex) : noteStr;
        int octave = 4; // Default octave if none is provided

        if (octaveIndex != -1 && !int.TryParse(noteStr.Substring(octaveIndex), out octave))
        {
            throw new ArgumentException($"Invalid octave format in: {noteStr}");
        }

        // 5. Inline dictionary map for note semitone offsets (C = 0)
        // Updated to include E#, B#, FB, and CB
        var noteOffsets = new Dictionary<string, int>
    {
        {"CB", -1}, // C-flat is semitone -1 (equivalent to B in the previous octave)
        {"C", 0},   {"C#", 1},  {"DB", 1},
        {"D", 2},   {"D#", 3},  {"EB", 3},
        {"E", 4},   {"E#", 5},  // E-sharp is 5 (same as F)
        {"FB", 4},              // F-flat is 4 (same as E)
        {"F", 5},   {"F#", 6},  {"GB", 6},
        {"G", 7},   {"G#", 8},  {"AB", 8},
        {"A", 9},   {"A#", 10}, {"BB", 10},
        {"B", 11},  {"B#", 12}  // B-sharp is 12 (same as C in the next octave)
    };

        if (!noteOffsets.TryGetValue(noteName, out int noteOffset))
        {
            throw new ArgumentException($"Unknown note name: {noteName}");
        }

        // 6. Calculate steps from A4 (A4 is 57 semitones above C0)
        int totalSemitonesFromC0 = (octave * 12) + noteOffset;
        int semitonesFromA4 = totalSemitonesFromC0 - 57;

        // 7. Math formula: 440 * 2^(n/12) rounded to nearest integer
        double frequency = 440.0 * Math.Pow(2.0, semitonesFromA4 / 12.0);
        return (int)Math.Round(frequency);
    }
}

class Parser
{
    readonly StringBuilder sb = new();
    public Parser()
    {
        sb.Append("{");
    }
    public string Finish()
    {
        sb.AppendLine("};");
        return sb.ToString();
    }
    /*
    class Note
    {
      public:
        unsigned int x, freq;
        long timestamp, holdTime;
        Note(unsigned int x, long timestamp, long holdTime, unsigned int freq) : x(x), timestamp(timestamp), holdTime(holdTime), freq(freq) {}
        Note(unsigned int x, long timestamp, unsigned int freq) : base(x, timestamp, -1f, freq) {}
    };
    */
    public void Append(int x, int time, int hz, int holdTime)
    {
        sb.AppendLine($"Note({x}, {time}, {holdTime}, {hz}),");
    }
}









