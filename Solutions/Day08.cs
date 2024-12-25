using System.Globalization;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AOC_2015
{
    internal class Day08: BaseDayWithInput
    {
        public Day08()
        {

        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            foreach (var line in _input)
            {
                //Console.WriteLine(line);
                string lineStrip = line[1..^1];
                lineStrip = lineStrip.Replace("\\\\", "\\");
                lineStrip = lineStrip.Replace("\\\"", "\"");
                lineStrip = Regex.Replace(lineStrip, @"\\x([0-9a-f]{2})", delegate (Match m)
                { return ((char)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber)).ToString();});
                //Console.WriteLine(lineStrip+"\n---");
                ans += line.Length - lineStrip.Length;
            }
            return new($"{ans}");
        }
        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            foreach (var line in _input)
            {
                string lineEncode = line.Replace("\\", "\\\\");
                lineEncode = lineEncode.Replace("\"", "\\\"");
                lineEncode = $"\"{lineEncode}\"";
                ans += lineEncode.Length - line.Length;
            }
            return new($"{ans}");
        }
    }
}
