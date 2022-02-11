use milosp2022
create table predmet(
id int identity(1,1),
naziv nvarchar (30))

create table ucenik(
id int identity(1,1),
ime nvarchar (30), 
prezime nvarchar(30))

create table ocena(
id int identity(1,1),
ucenik_id int,
predmet_id int,
ocena int
)

select * from ocena 
join predmet on predmet.id = predmet_id
join ucenik on ucenik.id = ucenik_id

select ime+' '+prezime as ucenik, naziv, ocena from ocena 
join predmet on predmet.id = predmet_id
join ucenik on ucenik.id = ucenik_id
