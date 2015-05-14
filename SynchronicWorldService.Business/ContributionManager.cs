using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SynchronicWorldService.DataAccess;
using SynchronicWorldService.Utils;

namespace SynchronicWorldService.Business
{
    public class ContributionManager : IContributionManager
    {
        /// <summary>
        /// See interface
        /// </summary>
        public IUnitOfWork UoW { get; set; }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public Models.ServiceResponse<List<Contribution>> GetEventContributions(int eventId)
        {
            var svcResponse = new Models.ServiceResponse<List<Contribution>>();

            var eventt = UoW.Context.Events.Include(x => x.Contributions).FirstOrDefault(x => x.Id == eventId);
            if (eventt == null)
            {
                svcResponse.Report.ErrorList.Add(SWResources.Event_Not_Found);
            }
            else
            {
                svcResponse.Result = eventt.Contributions.ToList();
            }
            
            return svcResponse;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Models.ServiceResponse<List<Contribution>> GetPersonContributions(int userId)
        {
            var svcResponse = new Models.ServiceResponse<List<Contribution>>();

            var person = UoW.Context.People.Include(x => x.Contributions).FirstOrDefault(x => x.Id == userId);
            if (person == null)
            {
                svcResponse.Report.ErrorList.Add(SWResources.Person_Not_Found);
            }
            else
            {
                svcResponse.Result = person.Contributions.ToList();
            }

            return svcResponse;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Models.ServiceResponse<bool> DeleteAllPersonContributionsForOpenEvents(int userId)
        {
            var svcResponse = new Models.ServiceResponse<bool>{ Result = true };

            var person = UoW.Context.People.Include(x => x.Contributions.Select(y => y.Event).Select(y => y.EventStatus)).FirstOrDefault(x => x.Id == userId);
            if (person == null)
            {
                svcResponse.Report.ErrorList.Add(SWResources.Person_Not_Found);
                svcResponse.Result = false;
            }
            else
            {
                var personContribsForOpenEvents = person.Contributions.Where(x => x.Event.EventStatus.Code == Models.EventStatusCode.Open.ToString()).ToList();
                svcResponse.Report.InfoList.Add(String.Format(SWResources.DeleteAllPersonContributionsForOpenEvents_Success, personContribsForOpenEvents.Count));
                foreach (var contrib in personContribsForOpenEvents)
                {
                    UoW.Context.Contributions.Remove(contrib);
                }
            }

            return svcResponse;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="contrib"></param>
        /// <returns></returns>
        public Models.Contribution ConvertContribToWcfContrib(Contribution contrib)
        {
            if (contrib == null)
                return null;
            if (contrib.ContributionType == null)
                contrib.ContributionType = UoW.Context.ContributionTypes.First(x => x.Id == contrib.Fk_Type);
            return new Models.Contribution{
                Id = contrib.Id,
                Name = contrib.Name,
                Quantity = contrib.Quantity,
                FkEvent = contrib.Fk_Event,
                FkPerson = contrib.Fk_Person,
                Type = new Models.ContributionType
                {
                    Id = contrib.ContributionType.Id,
                    Code = contrib.ContributionType.Code,
                    Value = contrib.ContributionType.Value
                }
            };
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="contrib"></param>
        /// <returns></returns>
        public Contribution ConvertWcfContribToContrib(Models.Contribution contrib)
        {
            if (contrib == null)
                return null;
            return new Contribution
            {
                Id = contrib.Id,
                Name = contrib.Name,
                Quantity = contrib.Quantity,
                Fk_Type = contrib.Type.Id,
                ContributionType = new ContributionType
                {
                    Id = contrib.Type.Id,
                    Code = contrib.Type.Code,
                    Value = contrib.Type.Value
                },
                Fk_Person = contrib.FkPerson,
                Fk_Event = contrib.FkEvent
            };
        }
    }

    public interface IContributionManager
    {
        /// <summary>
        /// Unit of work
        /// </summary>
        IUnitOfWork UoW { get; set; }

        /// <summary>
        /// Retrieve all the contributions for an event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Models.ServiceResponse<List<Contribution>> GetEventContributions(int eventId);

        /// <summary>
        /// Retrieve all the contributions of a person (for all events)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Models.ServiceResponse<List<Contribution>> GetPersonContributions(int userId);

        /// <summary>
        /// Delete all the contributions of a person for all open events
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Models.ServiceResponse<bool> DeleteAllPersonContributionsForOpenEvents(int userId);

        /// <summary>
        /// Convert contribution to WCF contribution facade
        /// </summary>
        /// <param name="contrib"></param>
        /// <returns></returns>
        Models.Contribution ConvertContribToWcfContrib(Contribution contrib);

        /// <summary>
        /// Convert WCF contribution facade to contribution
        /// </summary>
        /// <param name="contrib"></param>
        /// <returns></returns>
        Contribution ConvertWcfContribToContrib(Models.Contribution contrib);
    }
}
