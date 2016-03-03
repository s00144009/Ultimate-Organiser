using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ultimateorganiser.Models
{
    public class ClubInitaliser: DropCreateDatabaseAlways<UltimateDb>
    {
        // Called on database setup
        protected override void Seed(UltimateDb context)
        {
            var initClubs = new List<Club>
            {
                //Chelsea
                new Club() { ClubName = "Chelsea" ,ClubDescription = "London Football Club" ,ClubEvents = new List<ClubEvent> {
                    new ClubEvent { EventTitle = "Training on pitch", EventDesc = "light training before game on sunday, bring water", EventDate = DateTime.Parse("08/03/2016"), EventLocation = "Sligo" ,eventType = EventType.Training , eventPriority = EventPriority.Low},
                    new ClubEvent { EventTitle = "Home Game", EventDesc = "Semi Final", EventDate = DateTime.Parse("11/03/2015"), EventLocation = "Sligo" , eventType = EventType.Game, eventPriority = EventPriority.Medium }
                },
                    ClubMembers = new List<ClubMember> {
                    new ClubMember { UserFName = "Eden", UserLName = "Hazard" , UserEmail = "Hazard@Chelsea.com" , UserDescription = "Footballer" , UserName = "ChelseaHazzard", UserDoB = DateTime.Parse("07/05/1994")},
                    new ClubMember { UserFName = "Diego", UserLName = "Costa" , UserEmail = "Costa@Chelsea.com" , UserDescription = "Footballer" , UserName = "ChelseaCosta", UserDoB = DateTime.Parse("07/05/1994")}
                } }, 

                //Manchester United
                new Club() { ClubName = "Machester" , ClubDescription = "Manchester Football Club", ClubEvents = new List<ClubEvent> {
                    new ClubEvent { EventTitle = "Training on pitch", EventDesc = "light training before game on sunday, bring water", EventDate = DateTime.Parse("08/03/2016"), EventLocation = "Sligo" , eventType = EventType.Training, eventPriority = EventPriority.Low },
                    new ClubEvent { EventTitle = "Home Game", EventDesc = "Semi Final", EventDate = DateTime.Parse("11/03/2016"), EventLocation = "Sligo" , eventType = EventType.Game, eventPriority = EventPriority.High }
                },
                    ClubMembers = new List<ClubMember> {
                    new ClubMember { UserFName = "Wayne", UserLName = "Rooney" , UserEmail = "Rooney@Arsenal.com" , UserDescription = "Footballer from England" , UserName = "ManchesterRooney", UserDoB = DateTime.Parse("07/05/1994")},
                    new ClubMember { UserFName = "David", UserLName = "deGea" , UserEmail = "deGea@Arsenal.com" , UserDescription = "Footballer from Spain" , UserName = "ManchesterdeGea", UserDoB = DateTime.Parse("07/05/1994")}
                } },

                //Celtic
                new Club() { ClubName = "Celtic", ClubDescription = "Scottish Football Club", ClubEvents = new List<ClubEvent> {
                    new ClubEvent { EventTitle = "Training on pitch", EventDesc = "light training before game on sunday, bring water", EventDate = DateTime.Parse("08/03/2016"), EventLocation = "Sligo" , eventType = EventType.Training, eventPriority = EventPriority.Medium },
                    new ClubEvent { EventTitle = "Home Game", EventDesc = "Semi Final", EventDate = DateTime.Parse("11/03/2016"), EventLocation = "Sligo" , eventType = EventType.Game, eventPriority = EventPriority.None }
                },
                    ClubMembers = new List<ClubMember>() {
                    new ClubMember {UserFName = "Henrick", UserLName = "Larson", UserEmail = "Lason@Celtic.com", UserDescription = "Swedish Footballer", UserName = "CelticLarson", UserDoB = DateTime.Parse("07/05/94")},
                    new ClubMember {UserFName = "Roy", UserLName = "Keane", UserEmail = "Keane@Celtic.com", UserDescription = "Irish Footballer", UserName = "CelticKeane", UserDoB = DateTime.Parse("07/05/94")},
                    new ClubMember {UserFName = "Anthony", UserLName = "Stokes", UserEmail = "Stoke@Celtic.com", UserDescription = "Irish Footballer", UserName = "CelticStoke", UserDoB = DateTime.Parse("07/05/94")}
                } },

                //Arsenal
                new Club() { ClubName = "Arsenal", ClubDescription = "London Football Club", ClubEvents = new List<ClubEvent> {
                    new ClubEvent { EventTitle = "Training on pitch", EventDesc = "light training before game on sunday, bring water", EventDate = DateTime.Parse("08/03/2016"), EventLocation = "Sligo" , eventType = EventType.Training, eventPriority = EventPriority.High },
                    new ClubEvent { EventTitle = "Home Game", EventDesc = "Semi Final", EventDate = DateTime.Parse("11/03/2016"), EventLocation = "Sligo" , eventType = EventType.Game, eventPriority = EventPriority.Low }
                },
                    ClubMembers = new List<ClubMember>() {
                    new ClubMember { UserFName = "Mesut", UserLName = "Ozil" , UserEmail = "Ozil@Arsenal.com" , UserDescription = "Footballer from Germany" , UserName = "ArsenalOzil", UserDoB = DateTime.Parse("07/05/1994")},
                    new ClubMember { UserFName = "Alexis", UserLName = "Sanchez" , UserEmail = "Alexis@Arsenal.com" , UserDescription = "Footballer from Chile", UserName = "ArsenalSanchez" , UserDoB = DateTime.Parse("07/05/1994")}
                } }


            };

            initClubs.ForEach(c => context.Clubs.Add(c));   // Add each Club to the dataabse

            

            context.SaveChanges();  // save changes to the database
        }

    }
}