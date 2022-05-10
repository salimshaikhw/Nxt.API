using AutoMapper;
using Microsoft.Extensions.Logging;
using Nxt.Common.Helpers.MemoryCache;
using Nxt.Entities.Dtos.Customer;
using Nxt.Entities.Models;
using Nxt.Repositories.Interfaces;
using Nxt.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nxt.Services
{
    public class CustomerService : Service, ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheHelper _memoryCacheHelper;

        public CustomerService(ILogger<CustomerService> logger, IMapper mapper, IUnitOfWork unitOfWork,
            IMemoryCacheHelper memoryCacheHelper)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _customerRepository = unitOfWork.Repository<Customer>();
            _memoryCacheHelper = memoryCacheHelper;
        }

        public async Task<IEnumerable<CustomerDetails>> GetCustomers()
        {
            try
            {
                //var query = _customerRepository.Query().Where(x => x.FullName.Contains(search));

                var customerDetails = _memoryCacheHelper.GetCache<IEnumerable<CustomerDetails>>("Customers");

                if (customerDetails == null)
                {
                    var customers = await _customerRepository.GetAllAsync();
                    customerDetails = _mapper.Map<IEnumerable<CustomerDetails>>(customers);

                    _memoryCacheHelper.SetCache<IEnumerable<CustomerDetails>>("Customers", customerDetails, TimeSpan.FromDays(1));
                }

                return customerDetails;
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<CustomerDetails> GetCustomer(int customerId)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(customerId);
                return _mapper.Map<CustomerDetails>(customer);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<CustomerDetails> CreateCustomer(CustomerInput customerInput)
        {
            try
            {
                customerInput.Validate();
                var customer = _mapper.Map<Customer>(customerInput);
                customer = await _customerRepository.CreateAsync(customer);
                await _unitOfWork.Commit();
                _logger.LogInformation($"Customer created successfully with id {customer.CustomerId}");
                return _mapper.Map<CustomerDetails>(customer);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<CustomerDetails> UpdateCustomer(int customerId, CustomerInput customerInput)
        {
            try
            {
                customerInput.Validate();

                var customer = await _customerRepository.FindByIdAsync(customerId);
                customer.FullName = customerInput.FullName;
                customer.Phone = customerInput.Phone;
                customer = await _customerRepository.UpdateAsync(customer);
                await _unitOfWork.Commit();
                _logger.LogInformation($"Customer updated successfully with id {customer.CustomerId}");
                return _mapper.Map<CustomerDetails>(customer);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<bool> DeleteCustomer(int customerId)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(customerId);
                await _customerRepository.DeleteAsync(customer);
                await _unitOfWork.Commit();
                _logger.LogInformation($"Customer deleted successfully with id {customer.CustomerId}");

                return true;
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }
    }
}
