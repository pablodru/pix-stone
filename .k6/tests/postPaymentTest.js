import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
	vus: 30,
	duration: "60s",
};

const keysData = new SharedArray("keys", function () {
	const result = JSON.parse(open("../seed/existing_keys.json"));
	return result;
});

const accountsData = new SharedArray("accounts", function () {
	const result = JSON.parse(open("../seed/existing_accounts.json"));
	return result;
});

const usersData = new SharedArray("users", function () {
	const result = JSON.parse(open("../seed/existing_users.json"));
	return result;
});

const banksData = new SharedArray("banks", function () {
	const result = JSON.parse(open("../seed/existing_banks.json"));
	return result;
});

export default function (){
    const randomKey = keysData[Math.floor(Math.random() * keysData.length)];
    const randomAccount = accountsData[Math.floor(Math.random() * accountsData.length)];
    const randomUser = usersData[Math.floor(Math.random() * usersData.length)];
    const randomBank = banksData[Math.floor(Math.random() * banksData.length)];

    const paymentToCreate = {
        origin:{
            user:{
                cpf: randomUser.CPF.toString()
            },
            account: {
                number: randomAccount.Number.toString(),
                agency: randomAccount.Agency.toString()
            }
        },
        destiny: {
            key: {
                value: randomKey.value,
                type: randomKey.Type
            }
        },
        amount: generateRandomNumber(1000, 100000)
    }
    const body = JSON.stringify(paymentToCreate);
    const headers = { "Content-Type": "application/json", "Authorization": `Bearer ${randomBank.Token}` };

    http.post(`http://localhost:5109/keys`, body, { headers });

    sleep(1);
}

function generateRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}