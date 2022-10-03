using AccountManagement.Application.Contracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUploader _fileUploader;

        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher, IFileUploader fileUploader)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateAccount command)
        {
            var operation = new OperationResult();
            if (_accountRepository.Exists(x => x.Username == command.Username
                || x.Mobile == command.Mobile))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var password = _passwordHasher.Hash(command.Password);
            var path = $"profilePhotos";
            var pictureName = _fileUploader.Upload(command.ProfilePhoto, path);
            var account = new Account(command.Fullname, command.Username, password, command.Mobile,
                command.RoleId, pictureName);

            _accountRepository.Create(account);
            _accountRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Edit(EditAccount command)
        {
            var operation = new OperationResult();

            var account = _accountRepository.Get(command.Id);
            if (account == null)
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }
            if (_accountRepository.Exists(x => (x.Username == command.Username
                                               || x.Mobile == command.Mobile)
                                               && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var path = $"profilePhotos";
            var pictureName = _fileUploader.Upload(command.ProfilePhoto, path);
            account.Edit(command.Fullname, command.Username, command.Mobile, command.RoleId, pictureName);

            _accountRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            var operation = new OperationResult();

            var account = _accountRepository.Get(command.Id);
            if (account == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            if (command.Password != command.RePassword)
            {
                return operation.Failed(ApplicationMessages.PasswordsNotMatch);
            }

            var password = _passwordHasher.Hash(command.Password);

            account.ChangePassword(password);
            _accountRepository.SaveChanges();

            return operation.Succedded();
        }

        public EditAccount GetDetails(long id)
        {
            return _accountRepository.GetDetails(id);
        }

        public List<AccountViewModel> Search(AccountSearchModel command)
        {
            return _accountRepository.Search(command);
        }
    }
}
