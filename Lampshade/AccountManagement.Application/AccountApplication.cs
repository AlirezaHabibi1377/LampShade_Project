using AccountManagement.Application.Contracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUploader _fileUploader;
        private readonly IAuthHelper _authHelper;
        private readonly IRoleRepository _roleRepository;

        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher, IFileUploader fileUploader, IAuthHelper authHelper, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _fileUploader = fileUploader;
            _authHelper = authHelper;
            _roleRepository = roleRepository;
        }

        public OperationResult Create(RegisterAccount command)
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

        public List<AccountViewModel> GetAccounts()
        {
            return _accountRepository.GetAccounts();
        }

        public List<AccountViewModel> Search(AccountSearchModel command)
        {
            return _accountRepository.Search(command);
        }

        public OperationResult Login(Login command)
        {
            var operation = new OperationResult();

            var account = _accountRepository.GetBy(command.Username);
            if (account == null)
            {
                return operation.Failed(ApplicationMessages.WrongUserPass);
            }

            (bool Verified, bool NeedsUpgrade) result = _passwordHasher.Check(account.Password, command.Password);

            if (!result.Verified)
            {
                return operation.Failed(ApplicationMessages.WrongUserPass);
            }

            var permissions = _roleRepository
                .Get(account.RoleId)
                .Permissions
                .Select(x => x.Code)
                .ToList();

            var user = new AuthViewModel(account.Id, account.RoleId, account.Fullname, account.Username, permissions);
            _authHelper.Signin(user);

            return operation.Succedded();
        }

        public void LogOut()
        {
            _authHelper.SignOut();
        }
    }
}
