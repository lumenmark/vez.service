﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using UsaWeb.Service.Data;
using UsaWeb.Service.Features.Extensions;
using UsaWeb.Service.Features.Requests;
using UsaWeb.Service.Features.Responses;
using UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.SurgicalSiteInfection.Implementations
{
    public class SurgicalSiteInfectionRepository(Usaweb_DevContext context) : ISurgicalSiteInfectionRepository
    {
        private readonly Usaweb_DevContext _context = context;

        public async Task<IEnumerable<SurgicalSiteInfectionResponse>> GetSurgicalSiteInfectionsAsync(SurgicalSiteInfectionRequest request)
        {
            var query = @"
                SELECT ssi.*, m.FullName, re.npi, re.nameWithDegree, re.resignedDt 
                FROM surgicalsiteinfection ssi 
                LEFT JOIN member m ON ssi.memberIdAssigned = m.memberId 
                LEFT JOIN (
                    SELECT DISTINCT npi, nameWithDegree, resignedDt 
                FROM reappointment    WHERE 
                    npi IS NOT NULL  -- Exclude NULL values
                ) re ON re.npi = ssi.NpiSurgeon1
                WHERE 1=1";

            var parameters = new List<SqlParameter>();

            if (request.SurgicalSiteInfectionId != null)
            {
                query += " AND ssi.surgicalSiteInfectionId = @SurgicalSiteInfectionId";
                AddSqlParameter(parameters, "@SurgicalSiteInfectionId", request.SurgicalSiteInfectionId);
            }

            if (request.EventDtStart != null)
            {
                query += " AND ssi.eventDt >= @EventDtStart";
                AddSqlParameter(parameters, "@EventDtStart", request.EventDtStart);
            }

            if (request.EventDtEnd != null)
            {
                query += " AND ssi.eventDt <= @EventDtEnd";
                AddSqlParameter(parameters, "@EventDtEnd", request.EventDtEnd);
            }

            if (!string.IsNullOrEmpty(request.ProviderName))
            {
                query += " AND m.FullName LIKE @ProviderName";
                AddSqlParameter(parameters, "@ProviderName", $"%{request.ProviderName}%");
            }

            if (request.StatusList != null && request.StatusList.Any())
            {
                query += " AND ssi.status IN (" + string.Join(",", request.StatusList.Select((s, i) => $"@statusList{i}")) + ")";
                for (int i = 0; i < request.StatusList.Count; i++)
                {
                    AddSqlParameter(parameters, $"@statusList{i}", request.StatusList[i].Trim());
                }
            }

            if (request.SurgeryList != null && request.SurgeryList.Any())
            {
                query += " AND ssi.surgicalProcedure IN (" + string.Join(",", request.SurgeryList.Select((s, i) => $"@surgeryList{i}")) + ")";
                for (int i = 0; i < request.SurgeryList.Count; i++)
                {
                    AddSqlParameter(parameters, $"@surgeryList{i}", request.SurgeryList[i].Trim());
                }
            }

            if (request.WoundClassificationList != null && request.WoundClassificationList.Any())
            {
                query += " AND ssi.woundClassification IN (" + string.Join(",", request.WoundClassificationList.Select((s, i) => $"@woundClassificationList{i}")) + ")";
                for (int i = 0; i < request.WoundClassificationList.Count; i++)
                {
                    AddSqlParameter(parameters, $"@woundClassificationList{i}", request.WoundClassificationList[i].Trim());
                }
            }

            // Add filter for Patient
            if (!string.IsNullOrEmpty(request.Patient))
            {
                query += @"
                    AND (
                        ssi.patientLastName LIKE @Patient 
                        OR ssi.patientFirstName LIKE @Patient 
                        OR ssi.FIN LIKE @Patient 
                        OR ssi.MRN LIKE @Patient
                    )";
                AddSqlParameter(parameters, "@Patient", $"%{request.Patient}%");
            }

            // Add filter for Surgeon
            if (!string.IsNullOrEmpty(request.Surgeon))
            {
                query += @"
                    AND (
                        m.FullName LIKE @Surgeon 
                        OR re.npi LIKE @Surgeon 
                    )";
                AddSqlParameter(parameters, "@Surgeon", $"%{request.Surgeon}%");
            }

            query += " ORDER BY ssi.surgicalSiteInfectionId DESC";

            var results = new List<SurgicalSiteInfectionResponse>();

            var connection = _context.Database.GetDbConnection();

            try
            {
                // Check if the connection is open
                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync(); // Open connection asynchronously
                }

                using var command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddRange(parameters.ToArray());

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    SurgicalSiteInfectionResponse response = await CreateSurgicalSiteInfectionResponseFromDbResult(reader);
                    results.Add(response);
                }
            }
            finally
            {
                // Only close the connection when all operations are completed
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }

            return results;
        }

        /// <summary>
        /// Creates the surgical site infection asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Please fill the form.</exception>
        /// <exception cref="System.InvalidOperationException">An error occurred while saving the data.</exception>
        public async Task<Models.SurgicalSiteInfection> CreateSurgicalSiteInfectionAsync(SurgicalSiteInfectionViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentException("Please fill the form.");
            }

            try
            {
                var entity = model.ToEntity();

                await _context.SurgicalSiteInfections.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                // Optionally log the exception and rethrow it
                throw new InvalidOperationException("An error occurred while saving the data.", ex);
            }
        }

        public async Task<bool> DeleteSurgicalSiteInfectionAsync(int id)
        {
            var entity = await _context.SurgicalSiteInfections.FindAsync(id);
            if (entity == null) return false;

            _context.SurgicalSiteInfections.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Models.SurgicalSiteInfection>> GetAllSurgicalSiteInfectionsAsync()
        {
            return await _context.SurgicalSiteInfections.ToListAsync();
        }

        public async Task<Models.SurgicalSiteInfection> GetSurgicalSiteInfectionByIdAsync(int id)
        {
            return await _context.SurgicalSiteInfections.FindAsync(id);
        }

        public async Task<Models.SurgicalSiteInfection> UpdateSurgicalSiteInfectionAsync(Models.SurgicalSiteInfection existingEntity)
        {
            // Start a transaction using the dbContext
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update the SurgicalSiteInfection entity
                _context.SurgicalSiteInfections.Update(existingEntity);
                await _context.SaveChangesAsync();

                // Fetch the existing SurgicalSiteInfectionSkinPrep entity
                var skinPrepExisted = await _context.SurgicalSiteInfectionSkinPreps
                    .FirstOrDefaultAsync(s => s.SurgicalSiteInfectionId == existingEntity.SurgicalSiteInfectionId);

                if (skinPrepExisted != null)
                {
                    // Update the existing SurgicalSiteInfectionSkinPrep entity
                    skinPrepExisted.SkinPrep = existingEntity.SkinPrep;
                    skinPrepExisted.CreateTs = DateTime.UtcNow;
                    _context.SurgicalSiteInfectionSkinPreps.Update(skinPrepExisted);
                }
                else
                {
                    // Create a new SurgicalSiteInfectionSkinPrep entity if none exists
                    var skinPrepEntity = new SurgicalSiteInfectionSkinPrep
                    {
                        CreateTs = DateTime.UtcNow,
                        SkinPrep = existingEntity.SkinPrep,
                        SurgicalSiteInfectionId = existingEntity.SurgicalSiteInfectionId
                    };
                    await _context.SurgicalSiteInfectionSkinPreps.AddAsync(skinPrepEntity);
                }

                // Commit the changes within the transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return existingEntity;
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of any error
                await transaction.RollbackAsync();
                throw new InvalidOperationException("An error occurred while updating the data.", ex);
            }
        }

        private static async Task<SurgicalSiteInfectionResponse> CreateSurgicalSiteInfectionResponseFromDbResult(DbDataReader reader)
        {
            return new SurgicalSiteInfectionResponse
            {
                SurgicalSiteInfectionId = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("surgicalSiteInfectionId")),
                Fin = await reader.IsDBNullAsync(reader.GetOrdinal("fin")) ? null : await reader.GetFieldValueAsync<int>(reader.GetOrdinal("fin")),
                Mrn = await reader.IsDBNullAsync(reader.GetOrdinal("mrn")) ? null : await reader.GetFieldValueAsync<int>(reader.GetOrdinal("mrn")),
                PatientFirstName = await reader.IsDBNullAsync(reader.GetOrdinal("patientFirstName")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("patientFirstName")),
                PatientLastMiddleName = await reader.IsDBNullAsync(reader.GetOrdinal("patientLastMiddleName")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("patientLastMiddleName")),
                PatientLastName = await reader.IsDBNullAsync(reader.GetOrdinal("patientLastName")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("patientLastName")),
                Dob = await reader.IsDBNullAsync(reader.GetOrdinal("dob")) ? null : DateOnly.FromDateTime(await reader.GetFieldValueAsync<DateTime>(reader.GetOrdinal("dob"))),
                Sex = await reader.IsDBNullAsync(reader.GetOrdinal("sex")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("sex")),
                AdmitDt = await reader.IsDBNullAsync(reader.GetOrdinal("admitDt")) ? null : DateOnly.FromDateTime(await reader.GetFieldValueAsync<DateTime>(reader.GetOrdinal("admitDt"))),
                AdmitNote = await reader.IsDBNullAsync(reader.GetOrdinal("admitNote")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("admitNote")),
                SurgicalProcedure = await reader.IsDBNullAsync(reader.GetOrdinal("surgicalProcedure")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("surgicalProcedure")),
                OutPatientInpatient = await reader.IsDBNullAsync(reader.GetOrdinal("outPatientInpatient")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("outPatientInpatient")),
                SurgeryDt = await reader.IsDBNullAsync(reader.GetOrdinal("surgeryDt")) ? null : DateOnly.FromDateTime(await reader.GetFieldValueAsync<DateTime>(reader.GetOrdinal("surgeryDt"))),
                EventDt = await reader.IsDBNullAsync(reader.GetOrdinal("eventDt")) ? null : DateOnly.FromDateTime(await reader.GetFieldValueAsync<DateTime>(reader.GetOrdinal("eventDt"))),
                SurgicalSiteInfectionType = await reader.IsDBNullAsync(reader.GetOrdinal("surgicalSiteInfectionType")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("surgicalSiteInfectionType")),
                PreOpAntibioticAdminNote = await reader.IsDBNullAsync(reader.GetOrdinal("preOpAntibioticAdminNote")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("preOpAntibioticAdminNote")),
                preOpAntibioticAdminYN = await reader.IsDBNullAsync(reader.GetOrdinal("preOpAntibioticAdminYN")) ? null : await reader.GetFieldValueAsync<bool>(reader.GetOrdinal("preOpAntibioticAdminYN")),
                SkinPrep = await reader.IsDBNullAsync(reader.GetOrdinal("skinPrep")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("skinPrep")),
                SurgicalProcedureRaw = await reader.IsDBNullAsync(reader.GetOrdinal("surgicalProcedureRaw")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("surgicalProcedureRaw")),
                SurgicalSiteInfectionTypeRaw = await reader.IsDBNullAsync(reader.GetOrdinal("surgicalSiteInfectionTypeRaw")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("surgicalSiteInfectionTypeRaw")),
                NpiSurgeon1 = await reader.IsDBNullAsync(reader.GetOrdinal("NpiSurgeon1")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("NpiSurgeon1")),
                NpiSurgeon2 = await reader.IsDBNullAsync(reader.GetOrdinal("NpiSurgeon2")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("NpiSurgeon2")),
                OrRoom = await reader.IsDBNullAsync(reader.GetOrdinal("orRoom")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("orRoom")),
                WoundClassification = await reader.IsDBNullAsync(reader.GetOrdinal("woundClassification")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("woundClassification")),
                Nhsn = await reader.IsDBNullAsync(reader.GetOrdinal("nhsn")) ? null : (await reader.GetFieldValueAsync<string>(reader.GetOrdinal("nhsn"))).Trim(), // Trimming the NHSN value
                NoteInline = await reader.IsDBNullAsync(reader.GetOrdinal("noteInline")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("noteInline")),
                Note = await reader.IsDBNullAsync(reader.GetOrdinal("note")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("note")),
                MemberIdCreatedBy = await reader.IsDBNullAsync(reader.GetOrdinal("memberIdCreatedBy")) ? null : await reader.GetFieldValueAsync<int>(reader.GetOrdinal("memberIdCreatedBy")),
                MemberIdAssigned = await reader.IsDBNullAsync(reader.GetOrdinal("memberIdAssigned")) ? null : await reader.GetFieldValueAsync<int>(reader.GetOrdinal("memberIdAssigned")),
                CreateTs = await reader.IsDBNullAsync(reader.GetOrdinal("createTs")) ? null : await reader.GetFieldValueAsync<DateTime>(reader.GetOrdinal("createTs")),
                Temp1 = await reader.IsDBNullAsync(reader.GetOrdinal("temp1")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("temp1")),
                Temp2 = await reader.IsDBNullAsync(reader.GetOrdinal("temp2")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("temp2")),
                Status = await reader.IsDBNullAsync(reader.GetOrdinal("status")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("status")),
                FullName = await reader.IsDBNullAsync(reader.GetOrdinal("FullName")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("FullName")),
                Npi = await reader.IsDBNullAsync(reader.GetOrdinal("npi")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("npi")),
                NameWithDegree = await reader.IsDBNullAsync(reader.GetOrdinal("nameWithDegree")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("nameWithDegree")),
                ResignedDt = await reader.IsDBNullAsync(reader.GetOrdinal("resignedDt")) ? null : await reader.GetFieldValueAsync<DateTime>(reader.GetOrdinal("resignedDt")),
                HairRemovalYn = await reader.IsDBNullAsync(reader.GetOrdinal("HairRemovalYn")) ? null : await reader.GetFieldValueAsync<bool>(reader.GetOrdinal("HairRemovalYn")),
                Asa = await reader.IsDBNullAsync(reader.GetOrdinal("asa")) ? null : await reader.GetFieldValueAsync<int>(reader.GetOrdinal("asa")),
                Icd10 = await reader.IsDBNullAsync(reader.GetOrdinal("icd10")) ? null : await reader.GetFieldValueAsync<string>(reader.GetOrdinal("icd10"))
            };
        }

        private static void AddSqlParameter(List<SqlParameter> parameters, string paramName, object value)
        {
            if (value != null)
            {
                parameters.Add(new SqlParameter(paramName, value));
            }
        }

        public async Task<IEnumerable<NhsnProcedureCategory>> GetNhsnProcedureCategories()
        {
            return await _context.NhsnProcedureCategories.OrderBy(_ => _.ProcedureCategoryName).ToListAsync();
        }
    }
}