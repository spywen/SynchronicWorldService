using System;
using System.ServiceModel;
using SynchronicWorldService.Models;

namespace SynchronicWorldService
{
    [ServiceContract]
    public interface IInfoService
    {
        /// <summary>
        /// Get database status with the number of entities inside each tables
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<String> GetDatabaseStatus();
    }
}
