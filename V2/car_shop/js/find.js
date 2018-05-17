module.exports = {};

/**
 * Maximum value that can be passed into search vector
 */
module.exports.MAX_VALUES = [5, 4, 4, 4, 1, 1, 1, 1];


/**
 * Finds cars that pass query and vector of parameters
 * @param {Collection<any>} collection Database collection.
 * @param {string} query String long at least 3 characters.
 * @param {number[]} vector Array of integers meaning option selected. 
 * @param {number} maxResults Number of results returned.
 * @return {Promise<{isWhole: boolean, items: any[]}>}
 */
module.exports.findCars = function(collection, query, vector, maxResults) {
    if (query.length < 3)
        return Promise.resolve({isWhole: true, results: []});
    
    // Equivalent of relational LIKE
    let dbQuery = [
      { name: {$regex:  '.*' + query + '.*', $options: 'i'} }
    ];
  
    switch(vector[0]) { // price 10 000 - 5 000 000, 6 options
      case 0:
        dbQuery.push({price: { $lte: 50000 }});
        break;
      case 1:
        dbQuery.push({price: { $gte: 50000, $lte: 150000 }});
        break;
      case 2:
        dbQuery.push({price: {$gte: 150000, $lte: 500000}});
      break;
      case 3:
        dbQuery.push({price: {$gte: 500000, $lte: 1000000}});
        break;
      case 4:
        dbQuery.push({price: {$gte: 1000000, $lte: 3000000}});
        break;
      case 5:
        dbQuery.push({price: {$gte: 3000000}});
    }
  
    switch(vector[1]) { // year 1960 - 2018, 5 options
      case 0:
        dbQuery.push({year: { $lte: 1980 }});
        break;
      case 1:
        dbQuery.push({year: { $gte: 1980, $lte: 1990 }});
        break;
      case 2:
        dbQuery.push({year: {$gte: 1990, $lte: 2000}});
      break;
      case 3:
        dbQuery.push({year: {$gte: 2000, $lte: 2010}});
        break;
      case 4:
        dbQuery.push({year: {$gte: 2010}});
    }
  
    switch(vector[2]) { // consumption 4 - 15, 6 options
      case 0:
        dbQuery.push({consumption: { $lte: 6 }});
        break;
      case 1:
        dbQuery.push({consumption: { $gte: 6, $lte: 8 }});
        break;
      case 2:
        dbQuery.push({consumption: {$gte: 8, $lte: 10}});
        break;
      case 3:
        dbQuery.push({consumption: {$gte: 10, $lte: 12}});
        break;
      case 4:
        dbQuery.push({consumption: {$gte: 12}});
    }
  
    if (vector[3] !== undefined && vector[3] !== null) // fuel, 5 options
      dbQuery.push({fuel: vector[3]});
    
    if (vector[4] !== undefined && vector[4] !== null) // parking assistant, 2 options
      dbQuery.push({parkingAssistant: !!vector[4]});
    
    if (vector[5] !== undefined && vector[5] !== null) // xenons, 2 options
      dbQuery.push({xenons: !!vector[5]});
  
    if (vector[6] !== undefined && vector[6] !== null) // ABS, 2 options
      dbQuery.push({ABS: !!vector[6]});
  
    if (vector[7] !== undefined && vector[7] !== null) // air conditioning, 2 options
      dbQuery.push({airConditioning: !!vector[7]});
  
    return collection
    .find({ $and: dbQuery })
    .limit(maxResults + 1)
    .toArray()
    .then(results => {
        const whole = (results.length < maxResults + 1);
        if (!whole)
          results.length = maxResults;
        
        return Promise.resolve({
            isWhole: whole,
            items: results
        });
    });
  }