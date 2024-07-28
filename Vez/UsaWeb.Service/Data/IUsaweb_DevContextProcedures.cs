﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using UsaWeb.Service.Models;

namespace UsaWeb.Service.Data
{
    public partial interface IUsaweb_DevContextProcedures
    {
        Task<List<sp_GetSS_CallsByMrnResult>> sp_GetSS_CallsByMrnAsync(int? mrn, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<sp_GetSS_RawResultResult>> sp_GetSS_RawResultAsync(string dateStart, string dateEnd, string status, string orderBy, string speciality, string surgeon, string dept, string statusfilter, string userAssignedFilter, string searchValue, string searchFirst, string searchLast, string includeExlude, bool? isArrival, bool? isNeedClearance, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<sp_GetSS_RawResultByMrnResult>> sp_GetSS_RawResultByMrnAsync(int? mrn, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<sp_GetWordsByFilterResult>> sp_GetWordsByFilterAsync(string sitename, string provider, string patientage, string patientsex, string specialty, string minutewaitexamroom, string minutewaitprovider, string datestart, string dateend, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<sp_GetWordsPieByFilterResult>> sp_GetWordsPieByFilterAsync(string sitename, string provider, string patientage, string patientsex, string specialty, string minutewaitexamroom, string minutewaitprovider, string datestart, string dateend, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
