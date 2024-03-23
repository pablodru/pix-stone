import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
	stages: [
        { duration: "30s", target: 10 },
        { duration: "20s", target: 15 },
        { duration: "10s", target: 20 },
    ],
};

const usersData = new SharedArray("users", function () {
	const result = JSON.parse(open("../seed/existing_users.json"));
	return result;
});

const banksData = new SharedArray("banks", function () {
	const result = JSON.parse(open("../seed/existing_banks.json"));
	return result;
});

export default function () {
	const randomUser = usersData[Math.floor(Math.random() * usersData.length)];
    const randomBank = banksData[Math.floor(Math.random() * banksData.length)];
	const keyToCreate = {
		key: {
            value: generateRandomNumber(10000000000, 99999999999).toString(),
            type: "Phone"
        },
        user: {
            cpf: randomUser.CPF.toString()
        },
        account: {
            number: generateRandomNumber(100000000, 999999999).toString(),
            agency: generateRandomNumber(1000, 9999).toString()
        }
	};
	const body = JSON.stringify(keyToCreate);
	const headers = { "Content-Type": "application/json", "Authorization": `Bearer ${randomBank.Token}` };

	http.post(`http://localhost:8080/keys`, body, { headers });

	//sleep(1);
}

function generateRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}