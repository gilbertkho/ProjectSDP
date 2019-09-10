drop table account cascade constraint purge;
drop table barang cascade constraint purge;
drop table d_barang cascade constraint purge;
drop table h_jual cascade constraint purge;
drop table d_jual cascade constraint purge;
drop table h_beli cascade constraint purge;
drop table d_beli cascade constraint purge;
drop table supplier cascade constraint purge;
drop table barang_supplier cascade constraint purge;

create table account
(
	username varchar2(20) constraint pk_acc primary key,
	password varchar2 (20) constraint nn_acc_pass not null,
	nama varchar2(50) constraint nn_acc_nama not null,
	tgllhr date constraint nn_acc_date not null,
	notelp varchar2(15) constraint nn_acc_notelp not null,
	gender varchar2(1) constraint ch_gender check(gender = 'M' or gender = 'F'),
    status varchar2(1) constraint ch_status check(status = 'C' or status = 'P' or status = 'K'),
    poin number(5) constraint ch_acc_poin check(poin >= 0)
);

create table barang
(
	id_barang varchar2(6) constraint pk_barang primary key,
	nama_barang varchar2 (50) constraint nn_barang_nama not null,
	fungsi varchar2(100) constraint nn_barang_fungsi not null,
	harga number(9) constraint nn_barang_harga not null
);

create table d_barang
(
	id_barang varchar2(6) constraint fk_d_barang references barang(id_barang),
    stock number(5) constraint ch_d_barang_stock check(stock >= 0),
	expired date constraint nn_d_barang_expired not null,
	jenis varchar2(10) constraint ch_dbarang_jenis check(jenis = 'box' or jenis = 'strip' or jenis = 'biji'),
	constraint pk_d_barang primary key(id_barang,expired,jenis)
);

create table supplier
(
    id_supplier varchar2(7) constraint pk_supplier primary key,
    nama_supplier varchar2(50) constraint nn_supplier_nama not null,
    alamat varchar2(50) constraint nn_supplier_alamat not null,
    notelp varchar2(15) constraint nn_supplier_notelp not null
);

create table barang_supplier	
(
    id_supplier varchar2(7) constraint fk_b_supplier_supplier references supplier(id_supplier),
    id_barang varchar2(6) constraint fk_b_supplier_barang references barang(id_barang),
    harga number(9) constraint ch_b_supplier_harga check (harga >= 0),
	constraint pk_barang_supplier primary key (id_supplier,id_barang)
);

create table h_beli
(
	nota_beli varchar2(10) constraint pk_h_beli primary key,
	id_supplier varchar2(7) constraint fk_h_beli_id_supplier references supplier(id_supplier),
	tanggal date constraint nn_h_beli_tanggal not null,
	total number(9) constraint nn_h_beli_total not null
);

create table d_beli
(
	nota_beli varchar2(10) constraint fk_d_beli_nota references h_beli(nota_beli),
	id_barang varchar2(6) constraint fk_d_beli_barang references Barang(id_barang),
	qty number(5) constraint ch_d_beli_qty check(qty >= 0),
	jenis varchar2(10) constraint ch_dbeli_jenis check(jenis = 'box' or jenis = 'strip' or jenis = 'biji'),
    constraint pk_d_beli primary key(nota_beli, id_barang, jenis)
);

create table h_jual
(
	nota_jual varchar2(10) constraint pk_h_jual primary key,
	username_cust varchar2(20) constraint fk_h_jual_cust references account(username),
	username_peg varchar2(20) constraint fk_h_jual_peg references account(username),
	tanggal date constraint nn_h_jual_tanggal not null,
	delivery varchar2(1) constraint ch_h_jual_delivery check(delivery = 1 or delivery = 0),
	tujuan varchar2(100),
	total number(9) constraint nn_h_jual_total not null
);

create table d_jual
(
	nota_jual varchar2(10) constraint fk_d_jual_nota references h_jual(nota_jual),
	id_barang varchar2(5) constraint fk_d_jual_barang references Barang(id_barang),
	qty number(5) constraint ch_d_jual_qty check(qty >= 0),
	jenis varchar2(10) constraint ch_djual_jenis check(jenis = 'box' or jenis = 'strip' or jenis = 'biji'),
    constraint pk_d_jual primary key(nota_jual, id_barang, jenis) 
);


--TRIGGER
--SUPPLIER
create or replace trigger tIdSupplier
before insert on supplier
for each row
declare
    temp number;
    total number;
    hasil varchar2(7);
	gabung varchar2(7);
begin
    gabung := 'S_';
    temp := instr(:new.nama_supplier,' ');
    if(temp = 0) then
        hasil := substr(:new.nama_supplier,1,2);
    else
        hasil := substr(:new.nama_supplier,1,1);
        hasil := hasil || substr(:new.nama_supplier,temp+1,1);
    end if;
    hasil := upper(hasil);
	
    select nvl(max(to_number(substr(id_supplier,5))),0) into temp from supplier where id_supplier like '%' || hasil || '%';
	
    if(temp=0)then
        hasil := hasil || '001';
    else
        temp := temp + 1;
        hasil := hasil || lpad(temp,3,'0');
    end if;
	gabung := gabung || hasil;
    :new.id_supplier := gabung;
end;
/
show err;

--BARANG
create or replace trigger tIdBarang
before insert on barang
for each row
declare
	temp number;
	total number;
	hasil varchar2(6);
begin
	temp := instr(:new.nama_barang,' ');
	if temp=0 then
		hasil := substr(:new.nama_barang,1,2);
	else
		hasil := substr(:new.nama_barang,1,1);
		hasil := hasil || substr(:new.nama_barang,temp+1,1);
	end if;
	hasil := upper(hasil);
	select count(*) into temp from barang where substr(id_barang,1,2)=hasil;
	if temp=0 then
		hasil := hasil || '001';
	else
		select max(substr(id_barang,3)) into total from barang where substr(id_barang,1,2)=hasil;
		total := total+1;
		hasil := hasil || lpad(total,3,'0');
	end if;
	:new.id_barang := hasil;
end;
/
show err;

--HJUAL
create or replace trigger tIdHjual
before insert on h_jual
for each row
declare
    angka number;
    urutan varchar2(3);
    gabung varchar2(5);
begin
    select nvl(max(to_number(substr(nota_jual,3))),0) into angka from h_jual;
    if angka = 0 then 
        angka := 1;
    else
        angka := angka + 1;
    end if;

    urutan := lpad(angka,3,0);
    gabung := 'NJ' || urutan;
    :new.nota_jual := gabung;
end;
/
show err;

--HBELI
create or replace trigger tIdHBeli
before insert on h_beli
for each row
declare
    angka number;
    urutan varchar2(3);
    gabung varchar2(5);
begin
    select nvl(max(to_number(substr(nota_beli,3))),0) into angka from h_beli;
    if angka = 0 then 
        angka := 1;
    else
        angka := angka + 1;
    end if;

    urutan := lpad(angka,3,0);
    gabung := 'NB' || urutan;
    :new.nota_beli := gabung;
end;
/
show err;

--PROCEDURE SHOW STOK BARANG
create or replace procedure procStok
is
	box number;
	strip number;
	biji number;
begin
	for i in (select * from barang) loop
	box := 0;
	strip := 0;
	biji := 0;
		for j in (select * from d_barang where id_barang = i.id_barang) loop
			if j.jenis = 'box' then box := box + j.stock; end if;
			if j.jenis = 'strip' then strip := strip + j.stock; end if;
			if j.jenis = 'biji' then biji := biji + j.stock; end if;
		end loop;
		dbms_output.put_line(i.id_barang || ' ' || box || ' ' || strip || ' ' || biji);
	end loop;
end;
/
show err;

create or replace function procStok
return varchar2
is
	box number;
	strip number;
	biji number;
	hasil varchar2(500);
begin
	for i in (select * from barang) loop
	box := 0;
	strip := 0;
	biji := 0;
		for j in (select * from d_barang where id_barang = i.id_barang) loop
			if j.jenis = 'box' then box := box + j.stock; end if;
			if j.jenis = 'strip' then strip := strip + j.stock; end if;
			if j.jenis = 'biji' then biji := biji + j.stock; end if;
		end loop;
		hasil := hasil || i.id_barang || '-' || i.nama_barang || '-' || box || '-' || strip || '-' || biji || '-';
	end loop;
	return hasil;
end;
/
show err;

--INSERT DATA
insert into account values('a','a','Andi',to_date('01-01-2019','DD-MM-YYYY'),'081548245120','M','C',100);
insert into account values('c','c','Cynthia',to_date('01-01-2019','DD-MM-YYYY'),'081548245120','M','P',0);
insert into account values('b','b','Budi',to_date('01-01-2019','DD-MM-YYYY'),'081548245120','F','K',200);

insert into barang values('','Panadol Batuk dan Flu','Obat Batuk dan Flu',10000);
insert into barang values('','Panadol Demam','Obat Demam',12000);
insert into barang values('','Diapet','Obat Diare',8000);

insert into d_barang values('PB001',5,to_date('01-02-2000','DD-MM-YYYY'),'box');
insert into d_barang values('PB001',5,to_date('01-02-2020','DD-MM-YYYY'),'box');
insert into d_barang values('PB001',5,to_date('01-02-2020','DD-MM-YYYY'),'strip');
insert into d_barang values('PB001',5,to_date('01-02-2020','DD-MM-YYYY'),'biji');
insert into d_barang values('PD001',10,to_date('01-02-2020','DD-MM-YYYY'),'strip');
insert into d_barang values('DI001',8,to_date('01-02-2020','DD-MM-YYYY'),'biji');

insert into h_jual values('','a','b',to_date('01-01-2019','DD-MM-YYYY'),1,'STTS',20000);
insert into h_jual values('','c','b',to_date('01-01-2019','DD-MM-YYYY'),1,'STTS',20000);
insert into h_jual values('','a','b',to_date('01-01-2019','DD-MM-YYYY'),1,'STTS',20000);

insert into d_jual values('NJ001','DI001',5,'box');
insert into d_jual values('NJ001','PD001',10,'biji');
insert into d_jual values('NJ002','PD001',10,'biji');
insert into d_jual values('NJ003','DI001',5,'strip');
insert into d_jual values('NJ003','PD001',10,'strip');

insert into supplier values('','farmasi','petemon','08123123182');
insert into supplier values('','farmasi','petemon','08123123182');
insert into supplier values('','farmasi jaya','petemon','08123123182');

insert into barang_supplier values('S_FA001','DI001',5000);
insert into barang_supplier values('S_FA001','PB001',5000);
insert into barang_supplier values('S_FA002','PD001',5000);

insert into h_beli values('','S_FA001',to_date('01-01-2019','DD-MM-YYYY'),20000);
insert into h_beli values('','S_FA001',to_date('01-01-2019','DD-MM-YYYY'),20000);
insert into h_beli values('','S_FA002',to_date('01-01-2019','DD-MM-YYYY'),20000);
              
insert into d_beli values('NB001','DI001',5,'box');
insert into d_beli values('NB001','PD001',10,'strip');
insert into d_beli values('NB002','PD001',10,'biji');
insert into d_beli values('NB003','DI001',5,'box');
insert into d_beli values('NB003','PD001',10,'biji');

commit;

-- Recent Updates
-- > Tambah Trigger ID Supplier
-- > Ganti Field ID Supplier ke varchar2(7), Format : S_<2 huruf pertama><3 angka index>
-- > tambah constraint foreign key ID Supplier di h beli 

// REVISI
menambah atribut complain pada h_beli
tambah tabel po atribut(requested name, kode po)
tambah tabel po_detail atribut(product name, quantity, kode po)


select sum(db.stock) from d_barang db where expired = (select min(expired) from d_barang where id_barang='PB001')