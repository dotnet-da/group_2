# group_2

# Description of the application (what it is used for):
We have created a little Pizza App Demo which you can use as 2 User types: 

 ### First the User (The one that orders the Pizza). 

As a User you can either login using your Account name and Password or create a new Account. Either way you will get to the “Costumer Window”. In there the User can see all the possible Pizzas that he can order on the left. If the User made its choice, he could select one of the Pizzas and click the Order button to send an Order to the Pizza-maker. From now on he can see his newly sent Order at the right of the Screen. If he then selects the Order, he can see the current Status of the Pizza. Pizzas-Orders start in the Status “Order given”, when the Pizza Maker selects to accept the order, the Order-status will display “Order accepted”, when the Pizza is fresh out of the Oven and ready to deliver the Order-status will display “Pizza ready” and at last, when the Pizza is delivered it will display “Order delivered”.

### Pizza Maker: 
When the Pizza-Maker logs in he will be greeted by a menu in which he can select to either go to his Ingredients Overview or go to his Orders. 
Let’s say we choose the Orders, then the Pizza-Maker gets to a window with all the currently available Orders displayed for him. From here he can select a Pizza and Confirm an Order, Make the Pizza, or Deliver the Pizza. Confirming the Order takes no Effort or Time, but making a Pizza takes a set amount of time and occupies the Pizza oven. Same with the Delivery Driver. So, the Pizza-Maker can make a Pizza and deliver one at the same time, but he can’t make or deliver 2 Pizzas at the same time. The progress is displayed with a little progress Bar. Making a Pizza subtracts a set amount of Ingredients form the inventory of the Pizza-Maker. 

If the Pizza-Maker clicks the ingredients button he will get to a Window in which he can overview all his available ingredient. If an Ingredient get low the Pizza-Maker can purchase a variable amount of this Ingredient on this Screen. 
Description of the database (ER-model & dump-file):
 

# Dump File: 
### With this Dump File you should be able to recreate the Database Structure

create table stjucloo.accounts
(
    ac_id       integer generated always as identity
        constraint pk_accounts
            primary key,
    ac_username varchar(50)  not null,
    ac_password varchar(255) not null,
    ac_type     varchar(50)  not null
);

alter table stjucloo.accounts
    owner to stjucloo;

grant select, update, usage on sequence stjucloo.accounts_ac_id_seq to dotnet_group2user;

create unique index accounts_pk
    on stjucloo.accounts (ac_id);

grant delete, insert, references, select, trigger, truncate, update on stjucloo.accounts to dotnet_group2user;

create table stjucloo.pizza
(
    p_id   integer generated always as identity
        constraint pk_pizza
            primary key,
    p_name varchar(50)
);

alter table stjucloo.pizza
    owner to stjucloo;

grant select, update, usage on sequence stjucloo.pizza_p_id_seq to dotnet_group2user;

create table stjucloo.backer_has_pizza
(
    p_id  integer not null
        constraint fk_backer_h_backer_ha_pizza
            references stjucloo.pizza
            on update restrict on delete restrict,
    ac_id integer not null
        constraint fk_backer_h_backer_ha_accounts
            references stjucloo.accounts
            on update restrict on delete restrict,
    constraint pk_backer_has_pizza
        primary key (p_id, ac_id)
);

alter table stjucloo.backer_has_pizza
    owner to stjucloo;

create unique index backer_has_pizza_pk
    on stjucloo.backer_has_pizza (p_id, ac_id);

create index backer_has_pizza_fk
    on stjucloo.backer_has_pizza (p_id);

create index backer_has_pizza2_fk
    on stjucloo.backer_has_pizza (ac_id);

grant delete, insert, references, select, trigger, truncate, update on stjucloo.backer_has_pizza to dotnet_group2user;

create table stjucloo.bestellungen
(
    backer_ac_id integer not null
        constraint fk_bestellu_backer_ha_accounts
            references stjucloo.accounts
            on update restrict on delete restrict,
    be_id        integer generated always as identity,
    ac_id        integer
        constraint fk_bestellu_kunde_has_accounts
            references stjucloo.accounts
            on update restrict on delete restrict,
    p_id         integer not null
        constraint fk_bestellu_consists__pizza
            references stjucloo.pizza
            on update restrict on delete restrict,
    be_backerid  integer not null,
    be_pizzaid   integer,
    be_ready     boolean default false,
    constraint pk_bestellungen
        primary key (backer_ac_id, be_id)
);

alter table stjucloo.bestellungen
    owner to stjucloo;

grant select, update, usage on sequence stjucloo.bestellungen_be_id_seq to dotnet_group2user;

create unique index bestellungen_pk
    on stjucloo.bestellungen (backer_ac_id, be_id);

create index kunde_has_bestellung_fk
    on stjucloo.bestellungen (ac_id);

create index consists_of_fk
    on stjucloo.bestellungen (p_id);

create index backer_has_bestellung_fk
    on stjucloo.bestellungen (backer_ac_id);

grant delete, insert, references, select, trigger, truncate, update on stjucloo.bestellungen to dotnet_group2user;

create unique index pizza_pk
    on stjucloo.pizza (p_id);

grant delete, insert, references, select, trigger, truncate, update on stjucloo.pizza to dotnet_group2user;

create table stjucloo.zutaten
(
    zu_id     integer generated always as identity,
    zu_name   varchar(50),
    zu_amount smallint
);

alter table stjucloo.zutaten
    owner to stjucloo;

grant select, update, usage on sequence stjucloo.zutaten_zu_id_seq to dotnet_group2user;

create table stjucloo.pizza_has_zutat
(
    p_id integer not null
        constraint myct1
            references stjucloo.pizza
            on update restrict on delete restrict,
    z_id integer not null
        constraint myct2
            references stjucloo.zutaten (zu_id)
            on update restrict on delete restrict,
    constraint pk_pizza_has_zutat
        primary key (p_id, z_id)
);

alter table stjucloo.pizza_has_zutat
    owner to stjucloo;

create unique index pizza_has_zutat_pk
    on stjucloo.pizza_has_zutat (p_id, z_id);

create index pizza_has_zutat_fk
    on stjucloo.pizza_has_zutat (p_id);

create index pizza_has_zutat2_fk
    on stjucloo.pizza_has_zutat (z_id);

grant delete, insert, references, select, trigger, truncate, update on stjucloo.pizza_has_zutat to dotnet_group2user;

create unique index zutaten_pk
    on stjucloo.zutaten (zu_id);

grant delete, insert, references, select, trigger, truncate, update on stjucloo.zutaten to dotnet_group2user;

# Instructions "how to install the application, so that a new developer can start to work":
The installation is really simple, just Download Visual Studio and clone the Project from https://github.com/dotnet-da/group_2 . From here on it is recommended that you start the Backend at least once, so that your Computer prompts the Question to trust the Certificate. After that you can just open a console, move to the Folder of the Backend and start it with the command “dotnet run”. 
After starting the Backend, you can now start the Frontend. On the Frontend-side nothing else is to do. You can now create a new User Account or login to the Pizzamaker Account with username: pizzabacker and password: 1234 . 

 
