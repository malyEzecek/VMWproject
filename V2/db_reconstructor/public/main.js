(function() {
    const URL = "/search";
    const HEADERS = {"Content-Type": "application/json"};
    const FUELS = ["Gas", "Diesel", "Electricity", "Hybrid", "LPG"];
    let tBody, form;

    const SELECT_IDS = ["price", "year", "consumption", "fuel"];
    const RADIO_NAMES = ["parking", "xenons", "ABS", "airConditioning"];

    function init() {
        tBody = document.querySelector("#results tbody");
        form = document.querySelector("form");

        form.onsubmit = function(event) {
            event.preventDefault();
            onSubmit();
        }
    }

    function clear() {
        tBody.innerHTML = "";
    }

    function getIDElem(id) {
        let elem = document.createElement("th");
        elem.scope = "row";
        elem.innerText = id;

        return elem;
    }

    function getCell(content) {
        let elem = document.createElement("td");
        elem.innerText = content;
        
        return elem;
    }

    function getCar(car) {
        let tr = document.createElement("tr");
        tr.appendChild(getIDElem(car._id));
        tr.appendChild(getCell(car.name));
        tr.appendChild(getCell(car.price));
        tr.appendChild(getCell(car.year));
        tr.appendChild(getCell(car.consumption));
        tr.appendChild(getCell(FUELS[car.fuel]));
        tr.appendChild(getCell(car.parkingAssistant));
        tr.appendChild(getCell(car.xenons));
        tr.appendChild(getCell(car.ABS));
        tr.appendChild(getCell(car.airConditioning));

        return tr;
    }

    function toValue(i) {
        if (i == 0)
            return null;
        else
            return i - 1;
    }

    function radioValue(id) {
        console.log(`[name="${id}"][checked]`);
        let i = document.querySelector(`[name="${id}"]:checked`).value;
        return toValue(i);
    }

    function selectValue(id) {
        let elem = document.getElementById(id);
        return toValue(elem.selectedIndex);
    }

    function onSubmit() {
        const selects = SELECT_IDS.map(selectValue);
        const radios = RADIO_NAMES.map(radioValue);

        const name = document.getElementById("query").value;
        const vector = [...selects, ...radios];
        const size = +document.getElementById("size").value;
        const body = JSON.stringify({query: name, size: size, vector: vector});

        fetch(URL, {method: "POST", headers: HEADERS, body: body})
        .then(res => res.json())
        .then(results => {
            clear();
            results.items.forEach(car => {
                tBody.appendChild(getCar(car));
            })
        });
    }


    document.onreadystatechange = function() {
        if (document.readyState === "complete")
            init();
    };
})()