using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UsaWeb.Service.Models;
using UsaWeb.Service.Models.Ehr;

namespace UsaWeb.Service.Controllers
{
    [ApiController]
    public class EhrController : ControllerBase
    {
        string scope = "system/Account.read system/AllergyIntolerance.read system/Appointment.read system/Binary.read system/CarePlan.read system/CareTeam.read system/ChargeItem.read system/Communication.read system/Condition.read system/Consent.read system/Coverage.read system/Device.read system/DocumentReference.read system/Encounter.read system/FamilyMemberHistory.read system/Goal.read system/Immunization.read system/Location.read system/MedicationRequest.read system/NutritionOrder.read system/Observation.read system/Organization.read system/Patient.read system/Person.read system/Practitioner.read system/Procedure.read system/Provenance.read system/Questionnaire.read system/QuestionnaireResponse.read system/RelatedPerson.read system/Schedule.read system/ServiceRequest.read system/Slot.read";

        string tenantId = "ec2458f2-1e24-41c8-b71b-0e701af7583d"; //sandboxid


        [HttpGet("ehr/patient/patient_list")]
        public async Task<IActionResult> GetList(string searchValue, string searchLastName)
        {
            if (!string.IsNullOrEmpty(searchValue) && searchValue == "null") {
                searchValue = null;
            }
            if (!string.IsNullOrEmpty(searchLastName) && searchLastName == "null")
            {
                searchLastName = null;
            }

            EhrListModel_Message model = new EhrListModel_Message();
            List<EhrListModel> modelList = new List<EhrListModel>();

            var _token = await GetToken();

            //string uri = "https://fhir-open.cernweb.com/r4/ec2458f2-1e24-41c8-b71b-0e701af7583d/Patient?name=" + searchValue + "&_count=50";
            //string uri = "https://fhir-ehr-code.cernweb.com/r4/ec2458f2-1e24-41c8-b71b-0e701af7583d/Patient?name=" + searchValue + "&_count=50";

            string uri = null;
            if (!string.IsNullOrEmpty(searchValue) && string.IsNullOrEmpty(searchLastName))
                uri = string.Format("https://fhir-ehr-code.cernweb.com/r4/{0}/Patient?given={1}&_count=50", tenantId, searchValue);
            else if (string.IsNullOrEmpty(searchValue) && !string.IsNullOrEmpty(searchLastName))
                uri = string.Format("https://fhir-ehr-code.cernweb.com/r4/{0}/Patient?family={1}&_count=50", tenantId, searchLastName);
            else
                uri = string.Format("https://fhir-ehr-code.cernweb.com/r4/{0}/Patient?given={1}&family={2}&_count=50", tenantId, searchValue, searchLastName);

            //uri = string.Format("https://fhir-ehr-code.cernweb.com/r4/{0}/Patient?name={1}&_count=50", tenantId, searchValue);


            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + _token);

            using (var client = new HttpClient())
            {
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var dataString = response.Content.ReadAsStringAsync();
                    var result = dataString.Result;

                    var data = JObject.Parse(result);
                    var entries = data.SelectToken("entry");
                    if (entries != null)
                    {
                        foreach (var item in entries)
                        {
                            var modelItem = new EhrListModel();
                            var resource = item.SelectToken("resource");

                            var birthDate = string.Empty;
                            JToken bToken = resource["birthDate"];
                            if (bToken != null)
                            {
                                birthDate = bToken.ToString();
                            }

                            modelItem.id = resource["id"].ToString();
                            modelItem.active = bool.Parse(resource["active"].ToString());
                            modelItem.birthDate = birthDate;
                            modelItem.name = resource["name"][0]["text"].ToString();//resource["id"].ToString();
                            modelItem.mrn = resource["identifier"][0]["value"].ToString();
                            modelItem.cernId = resource["identifier"][0]["id"].ToString();

                            modelList.Add(modelItem);
                        }

                        model.List = modelList;
                        model.Message = "OK";
                    }
                    else
                    {
                        model.List = modelList;
                        model.Message = "ISSUE";
                    }
                }
                else 
                {
                    model.List = modelList;
                    model.Message = "ISSUE";
                }
            }
            return Ok(model);
        }


        [HttpGet("ehr/patient/patientbyidonebyone")]
        public async Task<IActionResult> GetPatientByIdOneByOne(string patientId, string resourceType, string _token)
        {
            //var _token = await GetToken();
            if(resourceType == "getnewtoken")
                _token = await GetToken();

            switch (resourceType)
            {
                case "MedicationRequest":
                    return Ok(new { medication = await GetResource(patientId, "MedicationRequest", _token) });
                case "Observation":
                    return Ok(new { observation = await GetResource(patientId, "Observation", _token) });
                case "Condition":
                    return Ok(new { condition = await GetResource(patientId, "Condition", _token) });
                case "Procedure":
                    return Ok(new { procedure = await GetResource(patientId, "Procedure", _token) });
                case "FamilyMemberHistory":
                    return Ok(new { familyMemberHistory = await GetResource(patientId, "FamilyMemberHistory", _token) });
                case "CarePlan":
                    return Ok(new { carePlan = await GetResource(patientId, "CarePlan", _token) });
                case "AllergyIntolerance":
                    return Ok(new { allergyIntolerance = await GetResource(patientId, "AllergyIntolerance", _token) });
                case "QuestionnaireResponse":
                    return Ok(new { questionnaireResponse = await GetResource(patientId, "QuestionnaireResponse", _token) });
                case "MedicationAdministration":
                    return Ok(new { medicationAdministration = await GetResource(patientId, "MedicationAdministration", _token) });
                case "Immunization":
                    return Ok(new { immunization = await GetResource(patientId, "Immunization", _token) });
                case "CareTeam":
                    return Ok(new { careTeam = await GetResource(patientId, "CareTeam", _token) });
                case "Goal":
                    return Ok(new { goal = await GetResource(patientId, "Goal", _token) });
                case "ServiceRequest":
                    return Ok(new { serviceRequest = await GetResource(patientId, "ServiceRequest", _token) });
                case "NutritionOrder":
                    return Ok(new { nutritionOrder = await GetResource(patientId, "NutritionOrder", _token) });
                default:
                    return Ok(new { patientDetail = await GetPatient(patientId, _token), });             
            }
        }

        [HttpGet("ehr/patient/patientbyid")]
        public async Task<IActionResult> GetPatientById(string patientId)
        {
            var _token = await GetToken();

            var result = new
            {
                patientDetail = await GetPatient(patientId, _token),
                medication = await GetResource(patientId, "MedicationRequest", _token),
                observation = await GetResource(patientId, "Observation", _token),
                condition = await GetResource(patientId, "Condition", _token),
                procedure = await GetResource(patientId, "Procedure", _token),
                familyMemberHistory = await GetResource(patientId, "FamilyMemberHistory", _token),
                carePlan = await GetResource(patientId, "CarePlan", _token),
                
                allergyIntolerance = await GetResource(patientId, "AllergyIntolerance", _token),
                questionnaireResponse = await GetResource(patientId, "QuestionnaireResponse", _token),
                medicationAdministration = await GetResource(patientId, "MedicationAdministration", _token),
                immunization = await GetResource(patientId, "Immunization", _token),
                careTeam = await GetResource(patientId, "CareTeam", _token),
                goal = await GetResource(patientId, "Goal", _token),
                serviceRequest = await GetResource(patientId, "ServiceRequest", _token),
                nutritionOrder = await GetResource(patientId, "NutritionOrder", _token),
            };
            return Ok(result);
        }
       
        [HttpGet("ehr/patient/gettoken")]
        public async Task<string> GetTokenGlobal()
        {
            string resultString = string.Empty;

            var Parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", scope)
            };

            // string uri = "https://authorization.cernweb.com/tenants/ec2458f2-1e24-41c8-b71b-0e701af7583d/protocols/oauth2/profiles/smart-v1/token";

            string uri = String.Format("https://authorization.cernweb.com/tenants/{0}/protocols/oauth2/profiles/smart-v1/token", tenantId);

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new FormUrlEncodedContent(Parameters)
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            request.Headers.TryAddWithoutValidation("Authorization", "Basic MjQwZjRlYmYtZDFhNC00YTAwLThlODEtMzMwYjE3ZmJiZDI3OlA3aS1hamx3cGFrTGRQV09saU5KckZRZkNzRTBDV0th");


            using (var client = new HttpClient())
            {
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var dataString = response.Content.ReadAsStringAsync();
                    var result = dataString.Result;

                    var data = JObject.Parse(result);
                    var access_token = data.SelectToken("access_token");

                    resultString = access_token.ToString();
                }
            }
            return resultString;
        }
        
        private async Task<string> GetPatient(string patientId, string _token)
        {
            string resultString = string.Empty;

            if (string.IsNullOrEmpty(_token))
                return null;

            //string uri = "https://fhir-open.cernweb.com/r4/ec2458f2-1e24-41c8-b71b-0e701af7583d/Patient?_id=" + patientId;
            //string uri = "https://fhir-ehr-code.cernweb.com/r4/ec2458f2-1e24-41c8-b71b-0e701af7583d/Patient?_id=" + patientId;

            string uri = String.Format("https://fhir-ehr-code.cernweb.com/r4/{0}/Patient?_id={1}", tenantId, patientId);


            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + _token);


            using (var client = new HttpClient())
            {
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var dataString = response.Content.ReadAsStringAsync();
                    var result = dataString.Result;

                    var data = JObject.Parse(result);
                    var entries = data.SelectToken("entry");

                    if (entries != null)
                    {
                        var resource = entries[0].SelectToken("resource");
                        resultString = resource.ToString();
                    }
                }
            }
            return resultString;
        }

        private async Task<string> GetResource(string patientId, string resourceName, string _token)
        {
            string resultString = string.Empty;


            //string uri = "https://fhir-open.cernweb.com/r4/ec2458f2-1e24-41c8-b71b-0e701af7583d/" + resourceName + "?patient=" + patientId;
            //            string uri = "https://fhir-ehr-code.cernweb.com/r4/ec2458f2-1e24-41c8-b71b-0e701af7583d/" + resourceName + "?patient=" + patientId;

            string uri = String.Format("https://fhir-ehr-code.cernweb.com/r4/{0}/{1}?patient={2}", tenantId, resourceName, patientId);


            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + _token);


            using (var client = new HttpClient())
            {
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var dataString = response.Content.ReadAsStringAsync();
                    var result = dataString.Result;

                    var data = JObject.Parse(result);
                    var entries = data.SelectToken("entry");

                    if (entries != null)
                    {
                        var resource = entries[0].SelectToken("resource");
                        resultString = resource.ToString();
                    }
                }
            }
            return resultString;
        }

        private async Task<string> GetToken()
        {
            string resultString = string.Empty;

            var Parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", scope)
            };

            // string uri = "https://authorization.cernweb.com/tenants/ec2458f2-1e24-41c8-b71b-0e701af7583d/protocols/oauth2/profiles/smart-v1/token";

            string uri = String.Format("https://authorization.cernweb.com/tenants/{0}/protocols/oauth2/profiles/smart-v1/token", tenantId);



            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new FormUrlEncodedContent(Parameters)
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            request.Headers.TryAddWithoutValidation("Authorization", "Basic MjQwZjRlYmYtZDFhNC00YTAwLThlODEtMzMwYjE3ZmJiZDI3OlA3aS1hamx3cGFrTGRQV09saU5KckZRZkNzRTBDV0th");


            using (var client = new HttpClient())
            {
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var dataString = response.Content.ReadAsStringAsync();
                    var result = dataString.Result;

                    var data = JObject.Parse(result);
                    var access_token = data.SelectToken("access_token");

                    resultString = access_token.ToString();
                }
            }
            return resultString;
        }



        [HttpGet("ehr/patient/{fin}")]
        public Array Mrn(string fin)
        {
            List<EhrModel> model = new List<EhrModel>();

            if (fin == "12345")
            {
                model.Add(new EhrModel
                {
                    fin = "12345",
                    mrn = "458347981",
                    first_name = "Bob",
                    last_name = "Jones",
                    birth_date = "2021-10-01",
                    contact = new Contact
                    {
                        Email = "bobjones@gmail.com",
                        Phone = "251-811-8811"
                    },
                    observation = new Observation
                    {
                        latest_Weight = new latest_weight
                        {
                            date_recorded = "1/13/21 11:00PM",
                            weight = "1.75",
                            unit_of_measure = "kg"
                        }
                    },
                    daily_schedule = new Daily_Schedule
                    {
                        feeding = new feeding
                        {
                            daytime = "8AM, 11AM, 2PM & 5PM",
                            nighttime = "11pm"
                        },
                        lab_work_scheduled = "blood work and complete blood count due at 5AM"
                    },


                });
            }
            else if (fin == "67890")
            {
                model.Add(new EhrModel
                {
                    fin = "67890",
                    mrn = "728347982",
                    first_name = "Jane",
                    last_name = "Jones",
                    birth_date = "2021-09-15",
                    contact = new Contact
                    {
                        Email = "janejones@gmail.com",
                        Phone = "251-617-87121"
                    },
                    observation = new Observation
                    {
                        latest_Weight = new latest_weight
                        {
                            date_recorded = "1/13/21 10:00PM",
                            weight = "1.70",
                            unit_of_measure = "kg"
                        }
                    },
                    daily_schedule = new Daily_Schedule
                    {
                        feeding = new feeding
                        {
                            daytime = "8AM, 11AM, 3PM",
                            nighttime = "11pm"
                        },
                        lab_work_scheduled = "blood work and complete blood count due at 6AM"
                    },


                });
            }




            return model.ToArray();
        }
    }
}
