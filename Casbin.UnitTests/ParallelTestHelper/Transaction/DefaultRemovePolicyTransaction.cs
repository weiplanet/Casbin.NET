using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Casbin.Model;
using Casbin.UnitTests.Extensions;

namespace Casbin.UnitTests.ParallelTest
{
    public class DefaultRemovePolicyTransaction<TRequest> : ITransaction<TRequest> where TRequest : IRequestValues
    {
        private List<TRequest> _requests = [];
        public bool ExpectedResult { get; private set; } = false;
        public bool ActualResult { get; private set; } = true;
        public bool HasCompleted { get; private set; } = false;
        public DefaultRemovePolicyTransaction(TRequest request)
        {
            _requests.Add(request);
        }
        public async Task<bool> ExecuteAsync(IConsumer<TRequest> consumer)
        {
            ActualResult = await consumer.RemovePolicyAsync(Request.First());
            HasCompleted = true;
            return true;
        }
        public void SetTruth(IEnforcer enforcer)
        {
            ExpectedResult = enforcer.RemovePolicy(Request.First().ToEnumerable());
        }
        public IEnumerable<TRequest> Request { get { return _requests; } }
        public TransactionType TransactionType { get; } = TransactionType.RemovePolicy;
    }
}