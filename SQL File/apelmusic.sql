CREATE DATABASE apelmusic;
USE apelmusic;

CREATE TABLE user_roles(
	role_id SMALLINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
	role_name VARCHAR(30)
);

CREATE TABLE apelmusic_user(
	user_id VARCHAR(36) PRIMARY KEY,
	user_name VARCHAR(70),
	user_email VARCHAR(255),
	user_password VARCHAR(255),
	role_id SMALLINT UNSIGNED,
	is_activated BOOL DEFAULT FALSE,
	
	FOREIGN KEY (role_id) REFERENCES user_roles(role_id)
);


CREATE TABLE class_category(
	category_id SMALLINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
	category_img VARCHAR(65530),
	category_name VARCHAR(25)
);

CREATE TABLE class(
	class_id SMALLINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
	class_category SMALLINT UNSIGNED,
	class_img VARCHAR(65530),
	class_name VARCHAR(100),
	class_description VARCHAR(255),
	class_price INT,
	class_status CHAR(8) DEFAULT 'active',
	
	FOREIGN KEY (class_category) REFERENCES class_category(category_id)
);





CREATE TABLE user_classes(
	user_class_id INT PRIMARY KEY AUTO_INCREMENT,
	user_id VARCHAR(36),
	class_id SMALLINT UNSIGNED,
	class_schedule DATETIME DEFAULT CURRENT_TIMESTAMP,
	is_paid BOOL DEFAULT FALSE,
	
	FOREIGN KEY (user_id) REFERENCES apelmusic_user(user_id),
	FOREIGN KEY (class_id) REFERENCES class(class_id)
);




CREATE TABLE orders (
	order_id VARCHAR(36) PRIMARY KEY,
	invoice_no VARCHAR(50) UNIQUE,
	order_date DATETIME DEFAULT CURRENT_TIMESTAMP,
	order_by VARCHAR(36),
	payment_method VARCHAR(30),
	total_price INT,
	is_paid BOOL DEFAULT TRUE,
	
	FOREIGN KEY (order_by) REFERENCES apelmusic_user(user_id)
);

CREATE TABLE order_detail(
	order_detail_id INT PRIMARY KEY AUTO_INCREMENT,
	invoice_no VARCHAR(50),
	user_class_id INT,
	
	FOREIGN KEY (invoice_no) REFERENCES orders(invoice_no),
	FOREIGN KEY (user_class_id) REFERENCES user_classes(user_class_id)
);

-- ==================================== Class Data ====================================
-- insert into class(class_category, class_img, class_name, class_description, class_price, class_status)
-- values (@class_category, @class_img, @class_name, @class_description, @class_price, @class_status);

-- update class
-- set class_category = @class_category, class_img = @class_img, class_name = @class_name, class_description = @class_description, class_price = @class_price, class_status = @class_status
-- where class_id = @class_id;

-- delete from class where class_id = @class_id;

-- ==================================== ========== ====================================
INSERT INTO class_category (category_img, category_name)
VALUES ("Category Test Image", "Drum"),
	("Category Test Image", "Bass"),
	("Category Test Image", "Guitar");

INSERT INTO class(class_category, class_img, class_name, class_description, class_price, class_status)
VALUES (1, "Class Test Image", "Class Dummer 1", "Kelas yang sangat mudah", 1000000, "active"),
	(3, "Class Test Image", "Class Guitar 1", "Kelas yang sangat mudah", 1000000, "active"),
	(2, "Class Test Image", "Class Bass 1", "Kelas yang sangat mudah", 1000000, "active");

INSERT INTO user_roles (role_name)
VALUES ("member");
-- ==================================== Class Category Data ====================================
-- insert into class_category (category_img, category_name)
-- values (@category_img, @category_name);

-- update class_category
-- set category_img = @category_img, category_name = @category_name
-- where category_id = @category_id;

-- delete from class_category where category_id = @category_id;

-- ==================================== Role ====================================
-- select * from user_roles;
-- select * from user_roles where role_id = @role_id;

-- insert into user_roles (role_name)
-- values (@role_name);

-- update user_roles set role_name = @role_name where role_id = @role_id;
-- delete from user_roles where role_id = @role_id;

-- select ur.role_name from user_roles ur
-- join apelmusic_user u on ur.role_id = u.@role_id;

-- ==================================== Create Account ====================================
-- INSERT INTO apelmusic_user (user_id, user_name, user_email, user_password, role_id)
-- VALUES (@user_id, @user_name, @user_email, @user_password, @role_id);



-- ==================================== Checkout flow ====================================
-- 1. Saat memasukan item ke keranjang
-- insert into user_chart (user_id, class_id)
-- values (//GUID USer, //CLASS ID);

	-- transaction  --

-- 2. User memilih item apa saja yang akan dimasukkan
-- insert into orders (order_id, invoice_no, order_by, payment_method, is_paid)
-- values (//ORDER ID, //INVOICE NO, //CURRENT USER ID, //PAYMENT METHOD, TRUE);

	-- Loop --
-- insert into order_detail (invoice_no, class_id)
-- values (//INVOICE YANG DIGENERATE SEBELUMNYA UNTUK TABLE ORDER, //CLASS ID YANG DIPILIH);


-- update user_classes isPaid

-- ==================================== Test Adding to user_classes ====================================
-- insert into user_classes (user_id, class_id, class_schedule, is_paid)
-- values (@user_id, @class_id, @class_schedule, false);

-- ==================================== Test Adding to orders, order_detail, confirm payment user_classes, and order total_price ====================================
-- insert into orders (order_id, invoice_no, order_by, payment_method)
-- values (@order_id, @invoice_no, @order_by, @payment_method);

-- insert into order_detail (invoice_no, class_id)
-- values (@invoice_no, @class_id);

-- update user_classes set is_paid = true
-- where user_id = @user_id and class_id = @class_id;

-- update orders
-- set total_price = (
--	select sum(c.class_price) from orders o
--	join order_detail od on o.invoice_no = od.invoice_no
--	join user_classes uc on od.user_class_id = uc.user_class_id
--	join class c on uc.class_id = c.class_id
--	where o.order_by = @user_id
--	group by o.invoice_no
--	having o.invoice_no = @invoice_no
-- )
-- where invoice_no = @invoice_no;

-- ==================================== Get Total in Order ====================================
-- select o.invoice_no, o.order_date, Count(od.order_detail_id) as "total_classes", sum(c.class_price) as "total_price" from orders o
-- join order_detail od on o.invoice_no = od.invoice_no
-- join user_classes uc on od.user_class_id = uc.user_class_id
-- join class c on uc.class_id = c.class_id
-- where o.order_by = @user_id
-- group by o.invoice_no;

-- ==================================== Get all for Order detail ====================================
-- select c.class_name, cc.category_name, uc.class_schedule, c.class_price from orders o
-- join order_detail od on o.invoice_no = od.invoice_no
-- join user_classes uc on od.user_class_id = uc.user_class_id
-- join class c on uc.class_id = c.class_id
-- join class_category cc on c.class_category = cc.category_id
-- where o.invoice_no = @invoice_no;

-- ==================================== Add all classes with category name ====================================
-- select c.*, cc.category_name from class c
-- join class_category cc on c.class_category = cc.category_id;

-- DROP DATABASE apelmusic;