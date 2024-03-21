import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

const keysData = new SharedArray("keys", function () {
    const result = JSON.parse(open("../seed/existing_keys.json"));
    return result;
});

const banksData = new SharedArray("banks", function () {
    const result = JSON.parse(open("../seed/existing_banks.json"));
    return result;
});

export const options = {
	vus: 30,
	duration: "60s",
};

export default function () {
    const randomKey = keysData[Math.floor(Math.random() * keysData.length)];
    const randomBank = banksData[Math.floor(Math.random() * banksData.length)];

    const headers = { 
        "Content-Type": "application/json", 
        "Authorization": `Bearer ${randomBank.Token}` 
    };

    const response = http.get(`http://localhost:5109/keys/${randomKey.Type}/${randomKey.Value}`, { headers });

}
