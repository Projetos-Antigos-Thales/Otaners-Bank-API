﻿using api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace api.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private ClientsRepository _repository = new ClientsRepository();

        // POST /Admins/Login + body = Login Method
        [EnableCors("AllowMyOrigin")]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] Client client)
        {
            try
            {
                string PASSWORD = "";

                try
                {
                    PASSWORD = _repository.SearchAccount(client.ACCOUNT).PASSWORD;
                    if (PASSWORD == "" || PASSWORD == string.Empty)
                    {
                        return NotFound("Account not found");
                    }
                }
                catch (Exception)
                {
                    return NotFound("Account not found");
                }


                if (PASSWORD == client.PASSWORD)
                {
                    return Ok();
                }
                else
                {
                    return Ok("Invalid Credentials");
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "We had an unknown error");
            }
        }

        // GET - /Clients/ClientsRegistered
        [EnableCors("AllowMyOrigin")]
        [HttpGet]
        public IActionResult CountClientsAccounts()
        {
            try
            {
                return Ok(_repository.CountClientsAccounts());
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred");
            }
        }

        // GET - /Clients/CPF = search a client by CPF
        [EnableCors("AllowMyOrigin")]
        [HttpGet("{CPF}")]
        public IActionResult SearchClient(string CPF)
        {
            var actionResult = _repository.SearchClient(CPF);
            if (actionResult == null)
            {
                return StatusCode(404, "Client not found");
            }
            else
            {
                return Ok(actionResult);
            }
        }

        // POST - /Clients + body = insert a new client
        [EnableCors("AllowMyOrigin")]
        [HttpPost]
        public IActionResult InsertClient([FromBody] Client client)
        {
            string actionResult = _repository.RegisterClient(client);

            if (actionResult == "200")
            {
                return Ok("Sucessfuly registered !");
            }
            else if (actionResult == "400")
            {
                return StatusCode(400, "Client already registered");
            }
            return StatusCode(500, "An error occurred");
        }

        // PUT - /Client/CPF + body = update a client
        [EnableCors("AllowMyOrigin")]
        [HttpPut("{CPF}")]
        public IActionResult UpdateClient([FromBody] Client client, string CPF)
        {
            string actionResult = _repository.UpdateClient(CPF, client);

            if (actionResult == "200")
            {
                return Ok("Updates sucessfuly applied !");
            }
            else if (actionResult == "404")
            {
                return StatusCode(404, "Client not found");
            }
            return StatusCode(500, "An error occurred");
        }

    }
}