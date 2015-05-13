﻿using System.ServiceModel;

namespace SynchronicWorldService
{
    [ServiceContract]
    public interface IPersonService
    {
        /// <summary>
        /// Create a person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<Models.Person> CreatePerson(Models.Person person);

        /// <summary>
        /// Get a person by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<Models.Person> GetPerson(int id);

        /// <summary>
        /// Update a person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<Models.Person> UpdatePerson(Models.Person person);

        /// <summary>
        /// Delete a person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<bool> DeletePerson(int id);
    }
}
