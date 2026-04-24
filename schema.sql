CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260329181946_InitialCreate') THEN
    CREATE TABLE "Products" (
        "Id" uuid NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Sku" text NOT NULL,
        "Price" numeric(18,2) NOT NULL,
        "CategoryId" integer NOT NULL,
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "DeletedAt" timestamp with time zone,
        "CreateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        "UpdateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        CONSTRAINT "PK_Products" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260329181946_InitialCreate') THEN
    CREATE TABLE "Sales" (
        "Id" uuid NOT NULL,
        "SaleId" uuid NOT NULL,
        "Date" timestamp with time zone NOT NULL,
        "Folio" text NOT NULL,
        "Total" numeric NOT NULL,
        "Status" integer NOT NULL,
        "CreateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        "UpdateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        CONSTRAINT "PK_Sales" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Sales_Sales_SaleId" FOREIGN KEY ("SaleId") REFERENCES "Sales" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260329181946_InitialCreate') THEN
    CREATE TABLE "Lots" (
        "Id" uuid NOT NULL,
        "ProductId" uuid NOT NULL,
        "InitialAmount" integer NOT NULL,
        "CurrentAmount" integer NOT NULL,
        "PurchaseCost" numeric NOT NULL,
        "ArrivateDate" timestamp with time zone NOT NULL,
        "Supplier" character varying(200),
        "IsActive" boolean NOT NULL,
        "CreateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        "UpdateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        CONSTRAINT "PK_Lots" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Lots_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260329181946_InitialCreate') THEN
    CREATE TABLE "SaleDetail" (
        "Id" uuid NOT NULL,
        "SaleId" uuid NOT NULL,
        "LotId" uuid NOT NULL,
        "ProductId" uuid NOT NULL,
        "Quantity" integer NOT NULL,
        "PriceAtSale" numeric NOT NULL,
        "CreateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        "UpdateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        CONSTRAINT "PK_SaleDetail" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_SaleDetail_Lots_LotId" FOREIGN KEY ("LotId") REFERENCES "Lots" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_SaleDetail_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_SaleDetail_Sales_SaleId" FOREIGN KEY ("SaleId") REFERENCES "Sales" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260329181946_InitialCreate') THEN
    CREATE INDEX "IX_Lots_ProductId" ON "Lots" ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260329181946_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_Products_Sku" ON "Products" ("Sku");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260329181946_InitialCreate') THEN
    CREATE INDEX "IX_SaleDetail_LotId" ON "SaleDetail" ("LotId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260329181946_InitialCreate') THEN
    CREATE INDEX "IX_SaleDetail_ProductId" ON "SaleDetail" ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260329181946_InitialCreate') THEN
    CREATE INDEX "IX_SaleDetail_SaleId" ON "SaleDetail" ("SaleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260329181946_InitialCreate') THEN
    CREATE INDEX "IX_Sales_SaleId" ON "Sales" ("SaleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260329181946_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260329181946_InitialCreate', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260330214313_addVehicleTypeSocketType') THEN
    ALTER TABLE "Products" DROP COLUMN "CategoryId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260330214313_addVehicleTypeSocketType') THEN
    ALTER TABLE "Products" ADD "SocketTypeId" uuid;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260330214313_addVehicleTypeSocketType') THEN
    ALTER TABLE "Products" ADD "VehicleTypeId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260330214313_addVehicleTypeSocketType') THEN
    CREATE TABLE "SocketType" (
        "Id" uuid NOT NULL,
        "NameSocket" character varying(100) NOT NULL,
        "IsActive" boolean NOT NULL,
        "DeletedAt" timestamp with time zone,
        "CreateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        "UpdateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        CONSTRAINT "PK_SocketType" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260330214313_addVehicleTypeSocketType') THEN
    CREATE TABLE "VehicleType" (
        "Id" uuid NOT NULL,
        "NameVehicle" character varying(100) NOT NULL,
        "IsActive" boolean NOT NULL,
        "DeletedAt" timestamp with time zone,
        "CreateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        "UpdateAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        CONSTRAINT "PK_VehicleType" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260330214313_addVehicleTypeSocketType') THEN
    CREATE INDEX "IX_Products_SocketTypeId" ON "Products" ("SocketTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260330214313_addVehicleTypeSocketType') THEN
    CREATE INDEX "IX_Products_VehicleTypeId" ON "Products" ("VehicleTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260330214313_addVehicleTypeSocketType') THEN
    ALTER TABLE "Products" ADD CONSTRAINT "FK_Products_SocketType_SocketTypeId" FOREIGN KEY ("SocketTypeId") REFERENCES "SocketType" ("Id") ON DELETE SET NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260330214313_addVehicleTypeSocketType') THEN
    ALTER TABLE "Products" ADD CONSTRAINT "FK_Products_VehicleType_VehicleTypeId" FOREIGN KEY ("VehicleTypeId") REFERENCES "VehicleType" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260330214313_addVehicleTypeSocketType') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260330214313_addVehicleTypeSocketType', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260331211359_AddFkSaleDetails') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260331211359_AddFkSaleDetails', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260331212029_RemoveSaleId') THEN
    ALTER TABLE "Sales" DROP CONSTRAINT "FK_Sales_Sales_SaleId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260331212029_RemoveSaleId') THEN
    DROP INDEX "IX_Sales_SaleId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260331212029_RemoveSaleId') THEN
    ALTER TABLE "Sales" DROP COLUMN "SaleId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260331212029_RemoveSaleId') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260331212029_RemoveSaleId', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260401022239_AddReturnLogic') THEN
    ALTER TABLE "Sales" ALTER COLUMN "Total" TYPE numeric(18,2);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260401022239_AddReturnLogic') THEN
    ALTER TABLE "SaleDetail" ALTER COLUMN "PriceAtSale" TYPE numeric(18,2);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260401022239_AddReturnLogic') THEN
    ALTER TABLE "SaleDetail" ADD "ReturnedQuantity" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260401022239_AddReturnLogic') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260401022239_AddReturnLogic', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260403203318_UpdateLotsEntityRemoveArrivalDate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260403203318_UpdateLotsEntityRemoveArrivalDate', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260409042019_usersTable') THEN
    CREATE TABLE users (
        "Id" uuid NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Email" character varying(255) NOT NULL,
        "PasswordHash" text NOT NULL,
        "Role" text NOT NULL DEFAULT 'Employee',
        "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        CONSTRAINT "PK_users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260409042019_usersTable') THEN
    CREATE UNIQUE INDEX "IX_users_Email" ON users ("Email");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260409042019_usersTable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260409042019_usersTable', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260409224924_UnificarNombresAuditoria') THEN
    ALTER TABLE users RENAME COLUMN "UpdatedAt" TO "UpdatedoAt";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260409224924_UnificarNombresAuditoria') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260409224924_UnificarNombresAuditoria', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260410051902_deletedCreatedAtUser') THEN
    ALTER TABLE users RENAME COLUMN "UpdatedoAt" TO "UpdateAt";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260410051902_deletedCreatedAtUser') THEN
    ALTER TABLE users RENAME COLUMN "CreatedAt" TO "CreateAt";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260410051902_deletedCreatedAtUser') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260410051902_deletedCreatedAtUser', '10.0.5');
    END IF;
END $EF$;
COMMIT;

