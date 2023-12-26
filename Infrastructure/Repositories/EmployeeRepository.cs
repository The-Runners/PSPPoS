﻿using Contracts.DTOs.Employee;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Employee> _employees;
    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
        _employees = _context.Set<Employee>();
    }

    public async Task<EmployeeModelDto?> GetById(Guid id)
    {
        var employee = await _employees.FindAsync(id);
        if (employee == null)
        {
            return null;
        }

        return new EmployeeModelDto
        {
            Id = employee.Id,
            WorkStartTime = employee.WorkStartTime,
            WorkEndTime = employee.WorkEndTime,
        };
    }

    public IEnumerable<EmployeeModelDto?> GetAll()
    {
        var employees = _employees.ToList();
        var employeeDtos = new List<EmployeeModelDto>();
        foreach (var employee in employees)
        {
            var employeeDto = new EmployeeModelDto
            {
                Id = employee.Id,
                WorkStartTime = employee.WorkStartTime,
                WorkEndTime = employee.WorkEndTime,
            };
            employeeDtos.Add(employeeDto);
        }

        return employeeDtos;
    }

    public async Task Add(EmployeeCreateDto employeeDto)
    {
        var employee = new Employee()
        {
            Id = new Guid(),
            WorkStartTime = employeeDto.WorkStartTime,
            WorkEndTime = employeeDto.WorkEndTime,
        };
        await _employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task Update(EmployeeModelDto employeeDto)
    {
        var employee = new Employee()
        {
            Id = employeeDto.Id,
            WorkStartTime = employeeDto.WorkStartTime,
            WorkEndTime = employeeDto.WorkEndTime,
        };
        _employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var employee = await _employees.FindAsync(id);
        if (employee == null)
        {
            return;
        }

        _employees.Remove(employee);
        await _context.SaveChangesAsync();
    }
}