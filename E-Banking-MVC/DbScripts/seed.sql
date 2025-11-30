create table Account(
AccountId int primary key identity(1,1),
AccountHolder nvarchar(150) null,
AccountNumber nvarchar(100) null,
IsActive bit null,
HasOnlineBanking bit null
);

create table Payment(
PaymentId int identity(1,1) primary key,
AccountId int null,
Amount numeric null,
TransactionDate datetime null,
Purpose nvarchar(200) null,
PayerName nvarchar(200) null,
IsUrgent bit null,
foreign key(AccountId) references Account(AccountId) on delete cascade
);


insert into Account(AccountHolder, AccountNumber, IsActive, HasOnlineBanking) values  ('Pera Peric', '13512-18484-13218', 'TRUE', 'TRUE');
insert into Account(AccountHolder, AccountNumber, IsActive, HasOnlineBanking) values  ('Fakultet tehnickih nauka', '85435-18354-84121', 'TRUE', 'TRUE');

insert into Payment(AccountId,Amount,TransactionDate,Purpose,PayerName,IsUrgent) values (1, 5000, '2020-02-05 00:14:00', 'Dzeparac', 'Mika Mikic', 'FALSE');
insert into Payment(AccountId,Amount,TransactionDate,Purpose,PayerName,IsUrgent) values (2, 200, '2020-02-05 13:15:00', 'Prijava ispita', 'Laza Lazic', 'TRUE');
insert into Payment(AccountId,Amount,TransactionDate,Purpose,PayerName,IsUrgent) values (2, 2000, '2020-02-01 10:40:00', 'Overa semestra', 'Zika Zikic', 'TRUE');
