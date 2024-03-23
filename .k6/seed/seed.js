const dotenv = require("dotenv");
const faker = require("@faker-js/faker");
const fs = require("fs");
dotenv.config();

const knex = require("knex")({
	client: "pg",
	connection: process.env.DATABASE_URL,
});

const USERS = 1_000_000;
const BANKS = 1_000_000;
const ACCOUNTS = 1_000_000;
const KEYS = 1_000_000;
const PAYMENTS = 1_000_000;
const ERASE_DATA = true;

async function run() {
	if (ERASE_DATA) {
		await knex("Users").del();
		await knex("Banks").del();
		await knex("Accounts").del();
		await knex("Keys").del();
		await knex("Payments").del();
	}
	const start = new Date();

	const users = generateUsers();
	await populateUsers(users);
	generateJson("./seed/existing_users.json", users);

	const banks = generateBanks();
	await populateBank(banks);
	generateJson("./seed/existing_banks.json", banks);

	const accounts = generateAccounts(users, banks);
	await populateAccounts(accounts);
	generateJson("./seed/existing_accounts.json", accounts);

	const keys = generateKeys(accounts);
	await populateKeys(keys);
	generateJson("./seed/existing_keys.json", keys);

	const payments = generatePayments(accounts, keys);
	await populatePayments(payments);
	generateJson("./seed/existing_payments.json", payments);

	console.log("Closing DB connection...");
	await knex.destroy();

	const end = new Date();
	console.log("Done!");
	console.log(`Finished in ${(end - start) / 1000} seconds`);
}

run();

function generateUsers() {
	console.log(`Generating ${USERS} users...`);
	const users = [];
	for (let i = 0; i < USERS; i++) {
		users.push({
			Name: faker.fakerPT_BR.person.fullName(),
			CPF: faker.fakerPT_BR.number
				.int({ min: 10000000000, max: 99999999999 })
				.toString(),
			CreatedAt: new Date(),
			UpdatedAt: new Date(),
		});
	}

	return users;
}

function generateBanks() {
	console.log(`Generating ${BANKS} banks...`);
	const banks = [];
	for (let i = 0; i < BANKS; i++) {
		banks.push({
			Name: faker.fakerPT_BR.company.name(),
			Token: faker.fakerPT_BR.string.uuid(),
			WebHook: "http://localhost:5039",
			CreatedAt: new Date(),
			UpdatedAt: new Date(),
		});
	}
	return banks;
}

function generateAccounts(users, banks) {
	console.log(`Generating ${ACCOUNTS} accounts...`);
	const accounts = [];
	for (let i = 0; i < ACCOUNTS; i++) {
		const randomUser = users[Math.floor(Math.random() * users.length)];
		const randomBank = banks[Math.floor(Math.random() * banks.length)];
		accounts.push({
			Number: faker.fakerPT_BR.number
				.int({ min: 100000000, max: 999999999 })
				.toString(),
			Agency: faker.fakerPT_BR.number.int({ min: 1000, max: 9999 }).toString(),
			UserId: randomUser.Id,
			BankId: randomBank.Id,
			CreatedAt: new Date(),
			UpdatedAt: new Date(),
		});
	}
	return accounts;
}

function generateKeys(accounts) {
	console.log(`Generating ${KEYS} keys...`);
	const keys = [];
	for (let i = 0; i < KEYS; i++) {
		const randomAccount = accounts[Math.floor(Math.random() * accounts.length)];
		keys.push({
			Type: "Phone",
			Value: faker.fakerPT_BR.number
				.int({ min: 10000000000, max: 99999999999 })
				.toString(),
			AccountId: randomAccount.Id,
			CreatedAt: new Date(),
			UpdatedAt: new Date(),
		});
	}
	return keys;
}

function generatePayments(accounts, keys) {
	console.log(`Generating ${PAYMENTS} payments...`);
	const payments = [];
	const statusOptions = ["FAILED", "SUCCESS"];
	for (let i = 0; i < PAYMENTS; i++) {
		const randomAccount = accounts[Math.floor(Math.random() * accounts.length)];
		const randomKey = keys[Math.floor(Math.random() * keys.length)];
		const randomStatusIndex = Math.floor(Math.random() * statusOptions.length);
		const randomStatus = statusOptions[randomStatusIndex];
		payments.push({
			Status: randomStatus,
			Amount: faker.fakerPT_BR.number.int({ max: 15000 }).toString(),
			AccountId: randomAccount.Id,
			KeyId: randomKey.Id,
			CreatedAt: new Date(),
			UpdatedAt: new Date(),
		});
	}
	return payments;
}

async function populatePayments(payments) {
	console.log("Storing on DB...");
	const tableName = "Payments";
	const insertedIds = await knex
		.batchInsert(tableName, payments)
		.returning("Id");
	for (let i = 0; i < payments.length; i++) {
		payments[i].Id = insertedIds[i].Id;
	}
}

async function populateBank(banks) {
	console.log("Storing on DB...");

	const tableName = "Banks";
	const insertedIds = await knex.batchInsert(tableName, banks).returning("Id");
	for (let i = 0; i < banks.length; i++) {
		banks[i].Id = insertedIds[i].Id;
	}
}

async function populateUsers(users) {
	console.log("Storing on DB...");

	const tableName = "Users";
	const insertedIds = await knex.batchInsert(tableName, users).returning("Id");
	for (let i = 0; i < users.length; i++) {
		users[i].Id = insertedIds[i].Id;
	}
}

async function populateAccounts(accounts) {
	console.log("Storing on DB...");

	const tableName = "Accounts";
	const insertedIds = await knex
		.batchInsert(tableName, accounts)
		.returning("Id");
	for (let i = 0; i < accounts.length; i++) {
		accounts[i].Id = insertedIds[i].Id;
	}
}

async function populateKeys(keys) {
	console.log("Storing on DB...");

	const tableName = "Keys";
	const insertedIds = await knex.batchInsert(tableName, keys).returning("Id");
	for (let i = 0; i < keys.length; i++) {
		keys[i].Id = insertedIds[i].Id;
	}
}

function generateJson(filepath, data) {
	if (fs.existsSync(filepath)) {
		fs.unlinkSync(filepath);
	}
	fs.writeFileSync(filepath, JSON.stringify(data));
}
