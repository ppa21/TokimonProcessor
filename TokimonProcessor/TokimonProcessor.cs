using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TokimonProcessor {
    internal class TokimonProcessor {
        private static string name;
        private static string id;
        private static double score;
        private static string comment;
        private static string teamComment;
        private static string receivedFrom;
        private static int Length = 2;
        private static List<Tokimon> TokimonContainer = new List<Tokimon>();
        private static List<string> FileList = new List<string>();

        public static void Main(string[] args) {
            if (args.Length != 2) {
                throw new Exception("Wrong number of arguments");
            }

            string input = args[0];
            string output = args[1];

            if (!Directory.Exists(input)) {
                throw new Exception("Input Folder " + input + " does not exist");
            }

            if (!File.Exists(output)) {
                throw new Exception("Output Folder " + output + " does not exist");
            }

            SearchFiles(input);

            if (FileList.Count == 0) {
                throw new Exception("No json files found");
            }

            ReceivedFrom();

            TokimonWriter writer = new TokimonWriter(TokimonContainer);
            writer.WriteToFile(output);
        }

        private static void SearchFiles(string input) {
            string[] files = Directory.GetFiles(input, "*.json*", SearchOption.AllDirectories);

            foreach (string file in files) {
                FileList.Add(file);
                string content = File.ReadAllText(file);
                ReadFromJson(content, file);
            }
        }

        private static void ReadFromJson(string content, string file) {
            JObject jObject = JObject.Parse(content);

            for (int i = 0; i < jObject.Count - 1; i++) {
                name = (string) jObject["team"][i]["name"];
                id = (string) jObject["team"][i]["id"];
                score = (double) jObject["team"][i]["compatibility"]["score"];
                comment = (string) jObject["team"][i]["compatibility"]["comment"];
                teamComment = (string) jObject["extra_comments"];
                Tokimon toki = new Tokimon(name, id, score, comment, teamComment);
                
                // add error handling here
                
                TokimonContainer.Add(toki);
            }
        }

        private static void ReceivedFrom() {
            for (int i = 0; i < TokimonContainer.Count; i++) {
                int tmpFromId = TokimonContainer[i].ReceivedFrom();

                foreach (Tokimon toki in TokimonContainer) {
                    if (tmpFromId == toki.IntegerTokiId()) {
                        receivedFrom = toki.TokiId;
                    }
                }

                TokimonContainer[i].FromToki = receivedFrom;
            }
        }
    }
}