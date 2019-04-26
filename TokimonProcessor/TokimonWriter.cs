using System;
using System.Collections.Generic;
using System.IO;

namespace TokimonProcessor {
    public class TokimonWriter {
        private List<string> team;
        private List<Tokimon> tokimonContainer;

        public TokimonWriter(List<Tokimon> tokimonContainer) {
            this.tokimonContainer = tokimonContainer;
        }

        public void WriteToFile(string output) {
            StreamWriter writer = new StreamWriter(output);
            int count = 0;
            int teamCount = 0;

            ColumnDescription(writer);
            
            foreach(Tokimon toki in tokimonContainer) {
                string receivedFrom = toki.FromToki;
                string tokimonId = toki.TokiId;
                string score = toki.TokiScore.ToString();
                string comment = toki.TokiComment;
                string teamComment = toki.TeamComment;

                // if its from and to to the same tokimon, replace its name by dashes in the to field
                if(receivedFrom.Equals(tokimonId)) {
                    tokimonId = "---------";
                }

                team = new List<string>();

                teamCount = AddTeamNumber(writer, count, teamCount);

                team.Add("");
                team.Add(receivedFrom);
                team.Add(tokimonId);
                team.Add(score);
                team.Add(comment);
                team.Add("");

                AddTeamComment(count, teamComment);

                string data = string.Join(",", team);
                writer.Write(data);
                writer.Write("\n");

                count++;
            }

            writer.Close();
        }
        
        public void AddTeamComment(int count, string teamComment) {
            int currTokiId = 0;
            int nextTokiId = 0;
            int currRecFromId = 0;
            int nextRecFromId = 0;

            if(count < tokimonContainer.Count - 1) {
                currTokiId = tokimonContainer[count].TokimonTeamId();
                nextTokiId = tokimonContainer[count + 1].TokimonTeamId();
                currRecFromId = tokimonContainer[count].ReceivedFrom();
                nextRecFromId = tokimonContainer[count + 1].ReceivedFrom();
            }

            if (currTokiId != nextTokiId || count == tokimonContainer.Count - 1 || currRecFromId != nextRecFromId) {
                team.Add(teamComment);
            }
        }
        
        public int AddTeamNumber(StreamWriter writer, int count, int teamCount) {
            if(count < tokimonContainer.Count - 1 && (count - 1) > 0) {
                if(tokimonContainer[count].TokimonTeamId() !=
                   tokimonContainer[count - 1].TokimonTeamId()) {
                    teamCount++;
    
                    writer.Write("Team "+ teamCount);
                    writer.Write("\n");
                }
            } else if(count == 0) {
                teamCount++;
                writer.Write("Team " + teamCount);
                writer.Write("\n");
            }
    
            return teamCount;
        }
        
        public void ColumnDescription(StreamWriter writer) {
            team = new List<string>();

            team.Add("Team #");
            team.Add("From Toki");
            team.Add("To Toki");
            team.Add("Score");
            team.Add("Comment");
            team.Add("");
            team.Add("Extra");
    
            string data = string.Join(",", team);
            writer.Write(data);
            writer.Write("\n");
        }
    }
}