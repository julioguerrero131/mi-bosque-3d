﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO.IsolatedStorage;
using System;
using System.Net.Http;

public class Peticiones : MonoBehaviour
{
    public static Peticiones instance = new Peticiones();
    private const string url_padre = "https://mibosque.espol.edu.ec";
    private const string url_constructor = "/api/v1/";
    private const string url_registrar_estudiante = url_padre + url_constructor + "student/register";
    private bool checkConexionServer;

    public GameObject actionLogger;
    public ActionLogger AC;

    private void Start()
    {
        ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
        actionLogger = GameObject.Find("ActionLogger");
        AC = actionLogger.GetComponent<ActionLogger>();
        TryResend();
    }

    public void TryResend()
    {
        foreach (PeticionesPendientes pet in AC.actionLogger.peticiones)
        {
            if (pet.token!="" )
            {
                Debug.Log("enviando peticiones");
                bool sent=true;
                //try send
                if (pet.tipo=="prize")
                {
                    sent=resendPlayerPrize(pet.nombre, pet.token);
                }else if (pet.tipo =="start mision")
                {
                    sent = resendStartMission(pet.nombre, pet.token, pet.started);
                }
                else if (pet.tipo == "finish mision")
                {
                    sent = resendFinishMission(pet.token, pet.ended, Int32.Parse(pet.nombre));
                }
                else if (pet.tipo == "mision")
                {
                    sent = resendPlayerMission(pet.nombre, pet.token, pet.started,pet.ended);
                }
                else if (pet.tipo == "level")
                {
                    sent = resendGameLevel(pet.nombre, pet.token, pet.started, pet.ended);
                }
                Debug.Log("enviando:" + sent);

                if (!sent)
                {
                    AC.actionLogger.agregarAccion(pet.tipo, pet.nombre);
                }
            }
            else
            {
                Debug.Log("no se detecto token para peticiones");
                AC.actionLogger.agregarAccion(pet.tipo, pet.nombre);
            }
        }
        AC.actionLogger.clearPeticiones();
        AC.actionLogger.guardarPeticionesPendientes();

        foreach (Accion acc in AC.actionLogger.acciones)
        {
            //try send
        }
        AC.actionLogger.clearLog();
        AC.actionLogger.guardar();
    }

    private bool resendPlayerPrize(string prize, string playerData)
    {
        Debug.Log("enviando prize");
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre + url_constructor + "game/playerprize");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 3000;
        httpWebRequest.Headers["Authorization"] = playerData;
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string fecha = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                string json_text = "{\"prize\": \"" + prize +
                              "\", \"dateAquired\": \"" + fecha + "\" }"; ;


                streamWriter.Write(json_text);
                Debug.Log(url_padre + url_constructor + "game/playerprize");
                Debug.Log("Request formado: " + json_text);
            }

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);
            return true;
        }
        catch
        {
            
            return false;
        }
    }

    private bool resendPlayerMission(string mission, string playerData, string started, string ended = null)
    {
        Debug.Log("enviando mision");
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre + url_constructor + "game/playermission");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 3000;
        httpWebRequest.Headers["Authorization"] = playerData;
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json_text = "{\"mission\": \"" + mission +
                              "\", \"completed\": " + ((ended != null) ? "true" : "false") + ", " +
                              "\"dateStart\": \"" + started + "\"," +
                              "\"dateEnd\": \"" + ended + "\"}";


                streamWriter.Write(json_text);
                Debug.Log("Request formado: " + json_text);
            }

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);

            return true;

        }
        catch
        {
            
            return false;
        }
    }
    private bool resendGameLevel(string level, string playerData, string started, string ended = null)
    {
        Debug.Log("enviando level");
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre + url_constructor + "game/levelinstance");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 3000;
        httpWebRequest.Headers["Authorization"] = playerData;
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json_text = "{\"gameLevel\": \"" + level +
                    "\", \"dateStart\": \"" + started +
                    "\", \"dateEnd\": \"" + ended + "\"}";


                streamWriter.Write(json_text);
                Debug.Log("Request formado: " + json_text);
            }

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);


            return true;

        }
        catch
        {
           
            return false;
        }
    }

    private bool resendStartMission( string level, string playerData, string started)
    {
        Debug.Log("enviando start");
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre + url_constructor + "game/gameLevelInstance");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 3000;
        httpWebRequest.Headers["Authorization"] = playerData;
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json_text = "{\"gameLevel\": \"" + level +
                    "\", \"dateStart\": \"" + started +
                    "\"}";


                streamWriter.Write(json_text);
                Debug.Log("Request formado: " + json_text);
            }

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);


            return true;

        }
        catch
        {
            
            return false;
        }
    }


    private bool resendFinishMission(string playerData, string end, int levelId)
    {
        Debug.Log("enviando finish");
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre + url_constructor + "game/gameLevelInstance/" + levelId);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "PUT";
        httpWebRequest.Timeout = 3000;
        httpWebRequest.Headers["Authorization"] = playerData;
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json_text = "{\"completed\": \"" + 1 +
                    "\", \"dateEnd\": \"" + end +
                    "\"}";


                streamWriter.Write(json_text);
                Debug.Log("Request formado: " + json_text);
            }

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);
            
            return true;

        }
        catch
        {
            
            return false;
        }
    }

    public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }


    /* Registra el estudiante con los valores de playerData registrados en el formulario.
     */
    public JObject RegistrarEstudiante(PlayerData playerData)
    {

        Debug.Log("Registrar Estudiante---");
        Debug.Log(url_registrar_estudiante);
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_registrar_estudiante);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 3000;
        try
        {
            Debug.Log("inicio");
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                Debug.Log(playerData.genero);
                string genero="";
                if(playerData.genero.Equals("Niño"))
                {
                    genero = "M";
                }else if(playerData.genero.Equals("Niña"))
                {
                    genero = "F";
                }
                else if(playerData.genero.Equals("M") || playerData.genero.Equals("F"))
                {
                    genero = playerData.genero;
                }
                else
                {
                    genero = "NA";
                }
                string json = "{\"name\": " + "\"" + playerData.nombre + "\"," +
                          "\"age\": " +  + playerData.edad + "," +
                          "\"gender\": " + "\"" + genero + "\"," +
                          "\"code\":" + "\"" + playerData.unidadEducativa + "\" }";

                Debug.Log(json);
                
                streamWriter.Write(json);
                Debug.Log("Request formado: " + json);
            }

            var httpResponse = GetWebResponseNoException(httpWebRequest);
            Debug.Log(httpResponse);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Debug.Log("El response recibido es: ");
                Debug.Log(result);
                SaveStatus(result, "/responseRegister.json");
                return leerStatus("/responseRegister.json");
            }
        }
        catch (WebException error)
        {
            GameManager.OfflineMode = true;
            Debug.Log(error.ToString());
            string json = "{\"status\": 500 ," +
                      "\"error\": \"Servidor no responde a tiempo.\" }";
            /*ErrorMessage errorMsg = new ErrorMessage();
            errorMsg.status = 500;
            errorMsg.error = "Servidor no responde a tiempo.";
            string output = JsonConvert.SerializeObject(errorMsg);*/
            JObject json1 = JObject.Parse(json);
            if (!GameManager.OfflineMode)
                {
                actionLogger.GetComponent<ActionLogger>().actionLogger.agregarAccion("Settings", "Offline");
            }
            actionLogger.GetComponent<ActionLogger>().actionLogger.online = false;
            return json1;
        }
        catch
        {
            GameManager.OfflineMode = true;
            string json = "{\"status\": 500 ," +
                      "\"error\": \"Servidor no responde a tiempo.\" }";
            /*ErrorMessage errorMsg = new ErrorMessage();
            errorMsg.status = 500;
            errorMsg.error = "Servidor no responde a tiempo.";
            string output = JsonConvert.SerializeObject(errorMsg);*/
            JObject json1 = JObject.Parse(json);
            return json1;
        }
    }

    /* Almacena el contenido del Response en un JSON en la ruta persistente del proyecto.
     * Listado: https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
     */
    private void SaveStatus(string jsonResponse, string nameFile)
    {

        //StartCoroutine(checkInternetConnection(url_padre));
        /*StartCoroutine(CheckInternetConnection(url_padre));
        Debug.Log("Hay conexion al server?" + checkConexionServer);*/

        string path = GetPersistentDataPath(nameFile);
        Debug.Log(GetPersistentDataPath(""));
        File.WriteAllText(path, jsonResponse);
    }

    public void EraseStatus(string nameFile)
    {
        string json = "{\"status\": 500 }";
        string path = GetPersistentDataPath(nameFile);
        Debug.Log(GetPersistentDataPath(""));
        File.WriteAllText(path, json);
    }

    public JObject leerStatus(string nameFile)
    {
        Debug.Log(GetPersistentDataPath(""));
        //Debug.Log(GetPersistentDataPath(responseJson));
        string responseJson = readFromFile(nameFile);
        Debug.Log(nameFile);
        Debug.Log(responseJson);
        
        return getObjectResponseStudent(responseJson);
    }

    private string readFromFile(string filename)
    {
        string path = GetPersistentDataPath(filename);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not found!");
        }
        return "{}";
    }


    private string GetPersistentDataPath(string filename)
    {
        return Application.persistentDataPath + filename;
    }

    private JObject getObjectResponseStudent(string responseJson)
    {
        JObject json1 = JObject.Parse(responseJson);
        /*JToken status = json1.GetValue("status");
        int value = status.ToObject<int>();
        if (value == 201)
        {
            //var jsjs = json1["payload"]["StudentId"];
            /*Debug.Log("El StudentId: ");
            Debug.Log(jsjs);
        }
        else
        {
            JToken errorStudents = json1.GetValue("error");
            Debug.Log(errorStudents);
        }*/
        Debug.Log("Json terminado de leer");

        return json1;
    }


    //Incompleto
    public void login(PlayerData playerData)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre+url_constructor+"auth/game");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 3000;
        //httpWebRequest.Headers[""] = "";
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"username\": " + "\"" + playerData.UserName + "\"," +
                          "\"password\": \"" + playerData.PassWord + "\" }";

                streamWriter.Write(json);
                Debug.Log(url_padre + url_constructor + "auth/game");
                Debug.Log("Request formado: " + json);
            }

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);
            playerData.Token = jsonResponse["payload"]["token"].ToObject<string>();
        }
        catch
        {
            if (!GameManager.OfflineMode)
            {
                actionLogger.GetComponent<ActionLogger>().actionLogger.agregarAccion("Settings", "Offline");
            }
            actionLogger.GetComponent<ActionLogger>().actionLogger.online = false;
            GameManager.OfflineMode = true;
            string json = "{\"status\": 500 ," +
                      "\"error\": \"Servidor no responde a tiempo.\" }";
            /*ErrorMessage errorMsg = new ErrorMessage();
            errorMsg.status = 500;
            errorMsg.error = "Servidor no responde a tiempo.";
            string output = JsonConvert.SerializeObject(errorMsg);*/
            Debug.Log(json);
        }
    }

    private HttpWebResponse GetWebResponseNoException(HttpWebRequest req)
    {
        try
        {
            Debug.Log(req);
            return (HttpWebResponse)req.GetResponse();
        }
        catch (WebException we)
        {
            Debug.Log(we);
            var resp = we.Response as HttpWebResponse;
            if (resp == null)
                throw;
            return resp;
        }
    }

    public JObject registerPlayerPrize(string prize, PlayerData playerData)
    {
        return registerPlayerPrize(prize, playerData, true);
    }

    private JObject registerPlayerPrize(string prize, PlayerData playerData, bool isFirstTime)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre + url_constructor + "game/playerprize");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 3000;
        httpWebRequest.Headers["Authorization"] = playerData.Token;
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                
                string fecha = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                string json_text = "{\"prize\": \"" + prize +
                              "\", \"dateAquired\": \"" + fecha + "\" }"; ;
                
                
                streamWriter.Write(json_text);
                Debug.Log(url_padre + url_constructor + "game/playerprize");
                Debug.Log("Request formado: " + json_text);
            }

            HttpWebResponse httpResponse =  GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);
            if (jsonResponse.GetValue("status").ToObject<int>() == 400 && jsonResponse.GetValue("error").ToObject<string>().Contains("Token") && isFirstTime)
            {
                login(playerData);
                return registerPlayerPrize(prize, playerData, false);
            }

            return jsonResponse;

        }
        catch
        {
            ActionLogger ac = actionLogger.GetComponent<ActionLogger>();
            if (!GameManager.OfflineMode)
            {
                ac.actionLogger.agregarAccion("Settings", "Offline");
            }
            
            ac.actionLogger.online = false;
            ac.actionLogger.agregarPeticion("prize",prize, playerData.Token, System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),null);
            GameManager.OfflineMode = true;
            string json = "{\"status\": 500 ," +
                      "\"error\": \"Servidor no responde a tiempo.\" }";
            /*ErrorMessage errorMsg = new ErrorMessage();
            errorMsg.status = 500;
            errorMsg.error = "Servidor no responde a tiempo.";
            string output = JsonConvert.SerializeObject(errorMsg);*/
            JObject json1 = JObject.Parse(json);
            return json1;
        }
    }

    public JObject registerPlayerMission(string mission, PlayerData playerData, string started, string ended = null)
    {
        return registerPlayerMission(true, mission, playerData, started, ended);
    }

    private JObject registerPlayerMission(bool isFirstTime, string mission, PlayerData playerData, string started,  string ended = null)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre + url_constructor + "game/playermission");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 3000;
        httpWebRequest.Headers["Authorization"] = playerData.Token;
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json_text = "{\"mission\": \"" + mission +
                              "\", \"completed\": " + ((ended != null) ? "true" : "false" )+ ", " + 
                              "\"dateStart\": \"" + started + "\"," +
                              "\"dateEnd\": \"" + ended + "\"}";


                streamWriter.Write(json_text);
                Debug.Log("Request formado: " + json_text);
            }

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);
            if (jsonResponse.GetValue("status").ToObject<int>() == 400 && jsonResponse.GetValue("error").ToObject<string>().Contains("Token") && isFirstTime)
            {
                login(playerData);
                return registerPlayerMission(false, mission, playerData, started, ended);
            }

            return jsonResponse;

        }
        catch
        {
            actionLogger = GameObject.Find("ActionLogger");
            ActionLogger ac = actionLogger.GetComponent<ActionLogger>();
            if (!GameManager.OfflineMode)
            {
                ac.actionLogger.agregarAccion("Settings", "Offline");
            }

            ac.actionLogger.online = false;
            ac.actionLogger.agregarPeticion("mision", mission, playerData.Token, started, ended);
            try
            {
                actionLogger.GetComponent<ActionLogger>().actionLogger.online = false;
            }
            catch (Exception e)
            {
                Debug.Log("act logger component not found");
            }
            
            GameManager.OfflineMode = true;
            string json = "{\"status\": 500 ," +
                      "\"error\": \"Servidor no responde a tiempo.\" }";
            /*ErrorMessage errorMsg = new ErrorMessage();
            errorMsg.status = 500;
            errorMsg.error = "Servidor no responde a tiempo.";
            string output = JsonConvert.SerializeObject(errorMsg);*/
            JObject json1 = JObject.Parse(json);
            return json1;
        }
    }

    //CODIGO PARA OBTENER PREGUNTAS ACTUALIZADAS
    public JObject getPreguntas(PlayerData playerData)
    {
        Debug.Log("OBTENIENDO NUEVAS PREGUNTAS ");
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://mibosque.espol.edu.ec/api/v1/game/challenge");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "GET";
        httpWebRequest.Timeout = 6000;
        httpWebRequest.Headers["Authorization"] = playerData.Token;
        Debug.Log("Authorization: " + playerData.Token);
        try
        {

            /*using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json_text = "{\"mission\": \"" + mission +
                              "\", \"completed\": " + ((ended != null) ? "true" : "false") + ", " +
                              "\"dateStart\": \"" + started + "\"," +
                              "\"dateEnd\": \"" + ended + "\"}";


                streamWriter.Write(json_text);
                Debug.Log("Request formado: " + json_text);
            }*/

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);
            File.WriteAllText(Application.dataPath+ "/Resources/Questions/NuevasPreguntas3.json", jsonResponse["payload"].ToString());
            Debug.Log("json escrito y guardado ");
            /*if (jsonResponse.GetValue("status").ToObject<int>() == 400 && jsonResponse.GetValue("error").ToObject<string>().Contains("Token") && isFirstTime)
            {
                login(playerData);
                return registerPlayerMission(false, mission, playerData, started, ended);
                Debug.Log("Status 400 / error");
            }*/

            return jsonResponse;

        }
        catch
        {
            if (!GameManager.OfflineMode)
            {
                actionLogger.GetComponent<ActionLogger>().actionLogger.agregarAccion("Settings", "Offline");
            }
            try
            {
                actionLogger.GetComponent<ActionLogger>().actionLogger.online = false;
            }
            catch (Exception e)
            {
                Debug.Log("act logger component not found");
            }
            GameManager.OfflineMode = true;
            string json = "{\"status\": 500 ," +
                      "\"error\": \"Servidor no responde a tiempo.\" }";
            /*ErrorMessage errorMsg = new ErrorMessage();
            errorMsg.status = 500;
            errorMsg.error = "Servidor no responde a tiempo.";
            string output = JsonConvert.SerializeObject(errorMsg);*/
            JObject json1 = JObject.Parse(json);
            return json1;
        }
    }

    public JObject registerGameLevel(string level, PlayerData playerData, string started, string ended = null)
    {
        return registerGameLevel(true, level, playerData, started, ended);
    }

    private JObject registerGameLevel(bool isFirstTime, string level, PlayerData playerData, string started, string ended = null)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre + url_constructor + "game/levelinstance");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 3000;
        httpWebRequest.Headers["Authorization"] = playerData.Token;
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json_text = "{\"gameLevel\": \"" + level + 
                    "\", \"dateStart\": \"" + started + 
                    "\", \"dateEnd\": \"" + ended + "\"}";


                streamWriter.Write(json_text);
                Debug.Log("Request formado: " + json_text);
            }

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);
            if (jsonResponse.GetValue("status").ToObject<int>() == 400 && jsonResponse.GetValue("error").ToObject<string>().Contains("Token") && isFirstTime)
            {
                login(playerData);
                return registerGameLevel(false, level, playerData, started, ended);
            }

            return jsonResponse;

        }
        catch
        {
            ActionLogger ac = actionLogger.GetComponent<ActionLogger>();
            if (!GameManager.OfflineMode)
            {
                ac.actionLogger.agregarAccion("Settings", "Offline");
            }

            ac.actionLogger.online = false;
            ac.actionLogger.agregarPeticion("level", level, playerData.Token, started, ended);
            try
            {
                actionLogger.GetComponent<ActionLogger>().actionLogger.online = false;
            }
            catch (Exception e)
            {
                Debug.Log("act logger component not found");
            }
            GameManager.OfflineMode = true;
            string json = "{\"status\": 500 ," +
                      "\"error\": \"Servidor no responde a tiempo.\" }";
            /*ErrorMessage errorMsg = new ErrorMessage();
            errorMsg.status = 500;
            errorMsg.error = "Servidor no responde a tiempo.";
            string output = JsonConvert.SerializeObject(errorMsg);*/
            JObject json1 = JObject.Parse(json);
            return json1;
        }
    }

    public JObject registerStartMission(string level, PlayerData playerData, string started)
    {
        return registerStartMission(true, level, playerData, started);
    }

    private JObject registerStartMission(bool isFirstTime, string level, PlayerData playerData, string started)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre + url_constructor + "game/gameLevelInstance");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 3000;
        httpWebRequest.Headers["Authorization"] = playerData.Token;
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json_text = "{\"gameLevel\": \"" + level +
                    "\", \"dateStart\": \"" + started +
                    "\"}";


                streamWriter.Write(json_text);
                Debug.Log("Request formado: " + json_text);
            }

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);
            if (jsonResponse.GetValue("status").ToObject<int>() == 400 && jsonResponse.GetValue("error").ToObject<string>().Contains("Token") && isFirstTime)
            {
                login(playerData);
                return registerStartMission(false, level, playerData, started);
            }

            return jsonResponse;

        }
        catch
        {
            ActionLogger ac = actionLogger.GetComponent<ActionLogger>();
            if (!GameManager.OfflineMode)
            {
                ac.actionLogger.agregarAccion("Settings", "Offline");
            }

            ac.actionLogger.online = false;
            ac.actionLogger.agregarPeticion("start mision", level, playerData.Token, started, null);
            try
            {
                actionLogger.GetComponent<ActionLogger>().actionLogger.online = false;
            }
            catch (Exception e)
            {
                Debug.Log("act logger component not found");
            }
            GameManager.OfflineMode = true;
            string json = "{\"status\": 500 ," +
                      "\"error\": \"Servidor no responde a tiempo.\" }";
            /*ErrorMessage errorMsg = new ErrorMessage();
            errorMsg.status = 500;
            errorMsg.error = "Servidor no responde a tiempo.";
            string output = JsonConvert.SerializeObject(errorMsg);*/
            JObject json1 = JObject.Parse(json);
            return json1;
        }
    }

    public JObject registerFinishMission(PlayerData playerData, string end, int levelId)
    {
        return registerFinishMission(true, playerData, end, levelId);
    }

    private JObject registerFinishMission(bool isFirstTime,PlayerData playerData, string end, int levelId)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre + url_constructor + "game/gameLevelInstance/" + levelId);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "PUT";
        httpWebRequest.Timeout = 3000;
        httpWebRequest.Headers["Authorization"] = playerData.Token;
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json_text = "{\"completed\": \"" + 1 +
                    "\", \"dateEnd\": \"" + end +
                    "\"}";


                streamWriter.Write(json_text);
                Debug.Log("Request formado: " + json_text);
            }

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);
            if (jsonResponse.GetValue("status").ToObject<int>() == 400 && jsonResponse.GetValue("error").ToObject<string>().Contains("Token") && isFirstTime)
            {
                login(playerData);
                return registerFinishMission(false, playerData, end, levelId);
            }

            return jsonResponse;

        }
        catch
        {
            ActionLogger ac = actionLogger.GetComponent<ActionLogger>();
            if (!GameManager.OfflineMode)
            {
                ac.actionLogger.agregarAccion("Settings", "Offline");
            }

            ac.actionLogger.online = false;
            ac.actionLogger.agregarPeticion("finish mision", ""+levelId, playerData.Token, null, end);
            try
            {
                actionLogger.GetComponent<ActionLogger>().actionLogger.online = false;
            }
            catch (Exception e)
            {
                Debug.Log("act logger component not found");
            }
            GameManager.OfflineMode = true;
            string json = "{\"status\": 500 ," +
                      "\"error\": \"Servidor no responde a tiempo.\" }";
            /*ErrorMessage errorMsg = new ErrorMessage();
            errorMsg.status = 500;
            errorMsg.error = "Servidor no responde a tiempo.";
            string output = JsonConvert.SerializeObject(errorMsg);*/
            JObject json1 = JObject.Parse(json);
            return json1;
        }
    }
    public JObject registerPregunta(PlayerData playerData, string end, string option, string level,string code)
    {
        return registerPregunta(true, playerData, end, option, level, code);
    }

    private JObject registerPregunta(bool isFirstTime, PlayerData playerData, string end, string option, string level, string code)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_padre + url_constructor + "game/challenge");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "PUT";
        httpWebRequest.Timeout = 3000;
        httpWebRequest.Headers["Authorization"] = playerData.Token;
        /*
         * {
            "playerchallenges": [
                {
                    "challengeCodeName":"challenge_1",
                    "gameLevelName":"Bosque-Estación 4",
                    "optionCodeName": "option_1B",
                    "takenAt": "2022-01-20 22:39:01"
                }
            ]
        }
         */
        try
        {

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json_text = "{\"playerchallenges\": [{" +
                    " \"challengeCodeName\": \"" + code +
                     "\", \"gameLevelName\": \"" + level +
                      "\", \"optionCodeName\": \"" + option +
                      "\", \"takenAt\": \"" + end +
                    "\"}]}";


                streamWriter.Write(json_text);
                Debug.Log("Request formado: " + json_text);
            }

            HttpWebResponse httpResponse = GetWebResponseNoException(httpWebRequest);
            StreamReader stReader = new StreamReader(httpResponse.GetResponseStream());
            string responseText = stReader.ReadToEnd();
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Respuesta: " + responseText);
            if (jsonResponse.GetValue("status").ToObject<int>() == 400 && jsonResponse.GetValue("error").ToObject<string>().Contains("Token") && isFirstTime)
            {
                login(playerData);
                return registerPregunta(false, playerData, end, option, level, code);
            }

            return jsonResponse;

        }
        catch
        {
            /*ActionLogger ac = actionLogger.GetComponent<ActionLogger>();
            if (!GameManager.OfflineMode)
            {
                ac.actionLogger.agregarAccion("Settings", "Offline");
            }

            ac.actionLogger.online = false;
            ac.actionLogger.agregarPeticion("finish mision", "" + levelId, playerData.Token, null, end);
            try
            {
                actionLogger.GetComponent<ActionLogger>().actionLogger.online = false;
            }
            catch (Exception e)
            {
                Debug.Log("act logger component not found");
            }*/
            GameManager.OfflineMode = true;
            string json = "{\"status\": 500 ," +
                      "\"error\": \"Servidor no responde a tiempo.\" }";
            /*ErrorMessage errorMsg = new ErrorMessage();
            errorMsg.status = 500;
            errorMsg.error = "Servidor no responde a tiempo.";
            string output = JsonConvert.SerializeObject(errorMsg);*/
            JObject json1 = JObject.Parse(json);
            return json1;
        }
    }
}
