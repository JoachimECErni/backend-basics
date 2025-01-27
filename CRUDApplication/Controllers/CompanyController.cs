﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDApplication.Data;
using CRUDApplication.Domain.Entities;
using AutoMapper;
using CRUDApplication.Repositories;
using CRUDApplication.Domain.Contracts;
using CRUDApplication.Common;

namespace CRUDApplication.Controllers
{
    [ApiController]
    [Route("api/")]
    public class CompanyController : ControllerBase
    {
        private readonly IBaseRepository<Company> _context;
        private readonly IMapper _mapper;

        public CompanyController(IBaseRepository<Company> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("company")]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                var companies = await _context.GetAll(record => record.Games);
                var companiesDto = _mapper.Map<List<CompanyDto>>(companies);
                return Ok(companiesDto);
            }
            catch(Exception e)
            {
                Console.Error.WriteLine("Error occured: " + e.Message);
                Console.Error.WriteLine("Call stack: " + e.StackTrace);
                return StatusCode(500, "An Internal Server error occured");
            }
        }

        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            try
            {
                var company = await _context.Get(id, c => c.Games);
                var companyDto = _mapper.Map<CompanyDto>(company);
                return Ok(companyDto);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error occured: " + e.Message);
                Console.Error.WriteLine("Call stack: " + e.StackTrace);
                return StatusCode(500, "An Internal Server error occured");
            }
        }

        [HttpPost("company")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompany createCompany)
        {
            try
            {
                if (createCompany.CountryOfOrigin == countryOfOrigin.Unknown)
                    return BadRequest(new { message = "Invalid Country Code" });
                var company = _mapper.Map<Company>(createCompany);
                var createdCompany = await _context.Add(company);
                var companyDto = _mapper.Map<CompanyDto>(createdCompany);
                return CreatedAtAction(nameof(GetCompany), new {id=companyDto.Id}, companyDto);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error occured: " + e.Message);
                Console.Error.WriteLine("Call stack: " + e.StackTrace);
                return StatusCode(500, "An Internal Server error occured: ");
            }
        }

        [HttpPut("company")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] UpdateCompany updateCompany)
        {
            try
            {
                if (updateCompany.CountryOfOrigin == countryOfOrigin.Unknown)
                    return BadRequest(new { message = "Invalid Country Code" });
                var company = await _context.Get(id);
                if (company == null)
                    return NotFound();
                _mapper.Map(updateCompany, company);
                var updatedCompany = await _context.Update(company);
                var companyDto = _mapper.Map<CompanyDto>(updatedCompany);
                return Ok(companyDto);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error occured: " + e.Message);
                Console.Error.WriteLine("Call stack: " + e.StackTrace);
                return StatusCode(500, "An Internal Server error occured");
            }
        }

        [HttpDelete("company")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            try
            {
                var company = await _context.Delete(id);
                if (company == null)
                    return NotFound();
                var companyDto = _mapper.Map<CompanyDto>(company);
                return Ok(companyDto);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error occured: " + e.Message);
                Console.Error.WriteLine("Call stack: " + e.StackTrace);
                return StatusCode(500, "An Internal Server error occured");
            }
        }
    }
}
