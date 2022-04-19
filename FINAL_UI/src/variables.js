//const urlAPI = 'http://10.41.65.100:5000/api/';

const urlAPI = 'http://localhost:13197/api/';

export {
  urlAPI
};

/**
 *
 * select distinct pos.number, pos.shift, pl.productionline_name, pl.worksite_name, w.name, pos.created_at, pos.GMIDCode, pos.state, totalOperatingTime, totalNetOperatingTime, Availability, Performance, Quality, OLE, qtyProduced,
 *                workingDuration, product, prod.GMID, prod.BULK,  prod.family, prod.GIFAP, prod.description, prod.formulationType, prod.size, prod.idealRate, prod.bottlesPerCase,
 *                fillerCounter, caperCounter, weightBoxCounter, qualityControlCounter, fillerRejection, caperRejection, labelerRejection, weightBoxRejection, qualityControlRejection
 *                                 from dbo.ole_productionline pl, dbo.worksite w, dbo.ole_pos pos,
 *                                 dbo.ole_products prod, dbo.ole_rejection_counters rc
 *                                 where w.name = pl.worksite_name
 *                                 and pos.productionline_name = pl.productionline_name
 *                                 and pos.GMIDCode = prod.GMID
 *                                 and rc.po = pos.number
 *                                 and w.name = 'Cernay'
 *                                 and pl.productionline_name = 'F52'
 *
 *
 * select * from dbo.ole_productionline pl, dbo.worksite w, dbo.ole_pos pos,
 *                                 dbo.ole_products prod, dbo.ole_rejection_counters rc
 *                                 where w.name = pl.worksite_name
 *                                 and pos.productionline_name = pl.productionline_name
 *                                 and pos.GMIDCode = prod.GMID
 *                                 and rc.po = pos.number
 *                                 and w.name = 'Cernay'
 *                                 and pl.productionline_name = 'F52'
 */