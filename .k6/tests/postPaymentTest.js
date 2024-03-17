import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
	vus: 20,
	duration: "30s",
};

export default function (){

    const paymentToCreate = {
        origin:{
            user:{
                cpf: "15539456936"
            },
            account: {
                number: "523538312",
                agency: "9439"
            }
        },
        destiny: {
            key: {
                value: "76675758196",
                type: "Phone"
            }
        },
        amount: generateRandomNumber(1000, 100000)
    }
    const body = JSON.stringify(paymentToCreate);
    const headers = { "Content-Type": "application/json", "Authorization": `Bearer 49357fe7-17b8-4766-a69e-a0e3ddb4f8df` };

    http.post(`http://localhost:5109/payments`, body, { headers });

    sleep(1);
}

function generateRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}