using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace simulation_with_full_device_details
{
    using System.IO;
    using System.Runtime;

    public class Nodes
    {
        public Texture2D _node;
        public Vector2 nodePosition;
        public Texture2D _nodewire;
        public Vector2 wirePosition;

        //computer attributes:
        private bool Firewall;
        private bool AntiVirus;
        private bool EmailAndWebFilters;
        private bool UserSpecificFiltersPresent;
        private bool useOfEmailQuarentine;
        private int spamConfidenceLevel;

        //people attributes
        private string positionInCompany;
        private bool presentOnSocialMedia;
        private bool trained;
        private bool useOfRememberMyPassword;
        private bool useof2FA;

        //security scoring

        private int employmentScore;
        private int socialMediaPresent;
        private int trainedscore;
        private int usingRemembermypasswordScore;
        private int authenticationscore;      
        private int baseSecurityScore;
        private int filtersandQuarentineScore;
        private int userScore;
        private int deviceScore;
        private int maxscore;
        private string nodeSecurityLevel = "";
        
        Random rand = new Random();
        private int lineinProfiles;
        private int lineInDeviceSpecifications;

        //constructor
        public Nodes(Texture2D nodeTexture, int nodeNum)
        {
            _node = nodeTexture;
            int[] nodeXPos = { 123, 305, 440, 623, 785 };
            int[] nodeYPos = {427, 153, 427, 153, 427 };

            nodePosition = new Vector2(nodeXPos[nodeNum], nodeYPos[nodeNum]);
            spamConfidenceLevel = rand.Next(1,10);

            lineinProfiles = rand.Next(1, 17); //16 lines
            lineInDeviceSpecifications = rand.Next(1,16);//15 lines

            //one is the lowest score for any system
            employmentScore = 1;
            socialMediaPresent = 1;
            trainedscore = 1;
            usingRemembermypasswordScore = 1;
            authenticationscore = 1;
            baseSecurityScore = 1;
            filtersandQuarentineScore = 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_node, nodePosition, Color.White);
        }

        public float GetNodeXPos()
        {
            return nodePosition.X;
        }

        public float GetNodeYPos()
        {
            return nodePosition.Y;
        }

        public void GetPeopleDetails()
        {
            string[] splitDetails = new string[5];
            List<string> lineStorage = new List<string>();
            string line = "";

            using (StreamReader reader = new StreamReader("updatedProfiles.TXT"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    lineStorage.Add(line);
                }
            }

            splitDetails = lineStorage[lineinProfiles].Split(',');
            positionInCompany = splitDetails[0];
            presentOnSocialMedia = Convert.ToBoolean(splitDetails[1]);
            trained = Convert.ToBoolean(splitDetails[2]);
            useOfRememberMyPassword = Convert.ToBoolean(splitDetails[3]);
            useof2FA = Convert.ToBoolean(splitDetails[4]);
        }

        public void GetDeviceDetails()
        {
            string[] splitDetails = new string[5];
            List<string> lineStorage = new List<string>();
            string line = "";

            using (StreamReader reader = new StreamReader("updatedDeviceDetails.TXT"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    lineStorage.Add(line);
                }
            }

            splitDetails = lineStorage[lineInDeviceSpecifications].Split(',');
            Firewall = Convert.ToBoolean(splitDetails[0]);
            AntiVirus = Convert.ToBoolean(splitDetails[1]);
            EmailAndWebFilters = Convert.ToBoolean(splitDetails[2]);
            UserSpecificFiltersPresent = Convert.ToBoolean(splitDetails[3]);
            useOfEmailQuarentine = Convert.ToBoolean(splitDetails[4]);

        }

        public string DisplayNodeDetails()
        {
            return "User Details:\n Position in company: " + positionInCompany + "\n Present on social media: " + presentOnSocialMedia
                + "\n User trained? " + trained + "\n Two factor authentication in use? "+ useof2FA + "\n 'remember my password' used: " + useOfRememberMyPassword
                + "\n Device details: \n Firewall " + Firewall + "\n Antivirus in use " + AntiVirus + "\n Email and Web filters used: " + EmailAndWebFilters
                + "\n User specific filters used: " + UserSpecificFiltersPresent + "\n Email quarentine used: " + useOfEmailQuarentine + "\n Spam confidence Level: " + spamConfidenceLevel;
        }

        public int GetDeviceSecurityScore() //goes throgh and determines a secuirty rating out of 24 based on device attributes
        {
            if (Firewall == true && AntiVirus == true)
            {
                baseSecurityScore = 5;
            }
            else if (Firewall == true && AntiVirus == false)
            {
                baseSecurityScore = 4;
            }
            else if (Firewall == false && AntiVirus == true)
            {
                baseSecurityScore = 2;
            }
            



            if (EmailAndWebFilters == true && useOfEmailQuarentine == true)
            {
                filtersandQuarentineScore = 5;
            }
            else if (EmailAndWebFilters == true && useOfEmailQuarentine == false)
            {
                filtersandQuarentineScore = 4; // without quarentine the content is still stopped but admin can't check them                
            }
            //if there are no email and web filters then score is 0 - even with quarentine, no filters = no need for quarentine
            
            deviceScore =  spamConfidenceLevel + filtersandQuarentineScore + baseSecurityScore;

            return deviceScore;
        }

        public int getUserSecurityScore()  //works out a numerical security score out of 25
        {
            if (positionInCompany == "HR")
            {
                employmentScore = 2;
            }
            else if (positionInCompany == "manager" || positionInCompany == "Accountants")
            {
                employmentScore = 3;
            }
            else if (positionInCompany == "retail worker" || positionInCompany == "self employed")
            {
                employmentScore = 4;
            }
           

            if (presentOnSocialMedia == true)
            {
                socialMediaPresent = 5;                
            }

            if (trained == true)
            {
                trainedscore = 5;
            }            

            if (useOfRememberMyPassword == true)
            {
                usingRemembermypasswordScore = 3;
            }

            if (useof2FA == true)
            {
                authenticationscore = 5;
            }
            else
            {
                authenticationscore = 3;
            }

            userScore = employmentScore + socialMediaPresent + trainedscore + usingRemembermypasswordScore + authenticationscore;

            return userScore;
        }

        public void getNodeSecurityLevels() //works out the total secuirty score for the entire node
        {
            maxscore = userScore + deviceScore;
        }

        public void determineWhetherDeviceWillBeAttacked() //works out the user rating based on their score
        {
            if (maxscore == 10)
            {
                nodeSecurityLevel = "Critical";
            }
            else if (maxscore <= 28)
            {
                nodeSecurityLevel = "Severe";
            }
            else if (maxscore <= 30)
            {
                nodeSecurityLevel = "Moderate";
            }
            else if (maxscore <= 36)
            {
                nodeSecurityLevel = "low";
            }
            else
            {
                nodeSecurityLevel = "minor";
            }
        }
        public string displayStatusOfNodes()
        {
            return "Node Security level: " + nodeSecurityLevel;

        }
        



    }
}
