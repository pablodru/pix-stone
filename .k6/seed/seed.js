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
const ERASE_DATA = true;

async function run() {
	if (ERASE_DATA) {
		await knex("Users").del();
        await knex("Banks").del();
	}
	const start = new Date();

	const users = generateUsers();
	await populateUsers(users);
	generateJson("./seed/existing_users.json", users);

    const banks = generateBanks();
    await populateBank(banks)
    generateJson("./seed/existing_banks.json", banks);

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
			CPF: faker.fakerPT_BR.number.int({ min: 10000000000, max: 99999999999 })
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
			CreatedAt: new Date(),
			UpdatedAt: new Date(),
		});
	}
    return banks;
}

async function populateBank(banks) {
    console.log("Storing on DB...");

	const tableName = "Banks";
	await knex.batchInsert(tableName, banks);
}

async function populateUsers(users) {
	console.log("Storing on DB...");

	const tableName = "Users";
	await knex.batchInsert(tableName, users);
}

function generateJson(filepath, data) {
	if (fs.existsSync(filepath)) {
		fs.unlinkSync(filepath);
	}
	fs.writeFileSync(filepath, JSON.stringify(data));
}
