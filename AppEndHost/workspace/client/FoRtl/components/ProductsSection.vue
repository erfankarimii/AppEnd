<template>
    <section id="products-section" class="section bg-light">
        <div class="container">
            <h2 class="section-title">{{ translate("Products") }}</h2>
            <div v-if="loading" class="text-center py-5">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">{{ translate("Loading") }}...</span>
                </div>
            </div>
            <div v-else-if="products.length === 0" class="text-center py-5">
                <p class="text-muted">{{ translate("No products available") }}</p>
            </div>
            <div v-else class="row g-4">
                <div class="col-md-6 col-lg-4" v-for="product in products" :key="product.Id">
                    <div class="card product-card h-100">
                        <div class="product-image" v-if="product.Image">
                            <img :src="getImageSrc(product.Image)" class="card-img-top" :alt="product.Name" />
                        </div>
                        <div class="product-image d-flex align-items-center justify-content-center text-white" v-else>
                            <i class="fa-solid fa-box fa-4x"></i>
                        </div>
                        <div class="card-body">
                            <h5 class="card-title">{{ product.Name }}</h5>
                            <p class="card-text text-muted">{{ product.Description || translate("No description available") }}</p>
                            <div class="d-flex justify-content-between align-items-center">
                                <span class="h5 text-primary mb-0" v-if="product.Price">
                                    {{ formatNumber(product.Price) }} {{ translate("Toman") }}
                                </span>
                                <button class="btn btn-outline-primary btn-sm">
                                    {{ translate("View Details") }}
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
let _this = { 
    cid: "", 
    c: null,
    products: [],
    loading: true
};

export default {
    setup(props) { 
        _this.cid = props['cid']; 
    },
    data() { 
        return _this; 
    },
    created() { 
        _this.c = this; 
    },
    mounted() {
        this.loadProducts();
    },
    methods: {
        loadProducts() {
            _this.loading = true;
            console.log('[ProductsSection] Loading products...');
            if (typeof rpc === 'function') {
                rpc({
                    requests: [{ 
                        Method: "DefaultRepo.PublicMethods.GetProductsList", 
                        Inputs: {} 
                    }],
                    onDone: (responses) => {
                        console.log('[ProductsSection] RPC responses:', responses);
                        if (responses && responses.length > 0) {
                            const response = responses[0];
                            console.log('[ProductsSection] RPC response:', response);
                            
                            if (response.IsSucceeded && response.Result) {
                                const result = typeof R0R === 'function' ? R0R(responses) : response.Result;
                                console.log('[ProductsSection] Extracted result:', result);
                                
                                if (result && result.Master) {
                                    console.log(`[ProductsSection] Received ${result.Master.length} products`);
                                    _this.products = result.Master;
                                } else {
                                    console.warn('[ProductsSection] No Master data in result');
                                    _this.products = [];
                                }
                            } else {
                                console.error('[ProductsSection] RPC failed:', response.Result);
                                _this.products = [];
                            }
                        } else {
                            console.warn('[ProductsSection] No responses received');
                            _this.products = [];
                        }
                        _this.loading = false;
                    },
                    onFail: (error) => {
                        console.error("[ProductsSection] Error loading products:", error);
                        console.error("[ProductsSection] Error details:", {
                            message: error?.message,
                            stack: error?.stack
                        });
                        _this.loading = false;
                        _this.products = [];
                    }
                });
            } else {
                console.error("[ProductsSection] rpc is not available");
                _this.loading = false;
            }
        },
        translate(key) {
            if (typeof shared !== 'undefined' && typeof shared.translate === 'function') {
                return shared.translate(key);
            }
            return key;
        },
        formatNumber(num) {
            if (typeof shared !== 'undefined' && typeof shared.formatNumber === 'function') {
                return shared.formatNumber(num);
            }
            return num;
        },
        getImageSrc(imageBytes) {
            if (!imageBytes) return "/a..lib/images/avatar.png";
            if (typeof shared !== 'undefined' && typeof shared.getImageURI === 'function') {
                return shared.getImageURI(imageBytes);
            }
            // Fallback
            if (typeof imageBytes === 'string') {
                return 'data:image/png;base64, ' + imageBytes;
            }
            return "/a..lib/images/avatar.png";
        }
    },
    props: { cid: String }
}
</script>

<style scoped>
</style>

